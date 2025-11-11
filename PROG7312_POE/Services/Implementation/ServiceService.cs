using Microsoft.EntityFrameworkCore;
using PROG7312_POE.Models;
using PROG7312_POE.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PROG7312_POE.Services.Implementation
{
    public class ServiceService : IServiceService
    {
        private readonly AppDbContext _context;

        public ServiceService(AppDbContext context)
        {
            _context = context;
        }

        // ---------------------------------------------------------------------
        // Create service request
        public async Task<serviceTBL?> AddServiceAsync(serviceTBL service)
        {
            try
            {
                service.Title = service.Title?.Trim() ?? string.Empty;
                service.Description = service.Description?.Trim() ?? string.Empty;
                if (service.CreatedUtc == default) service.CreatedUtc = DateTime.UtcNow;

                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return service;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while adding service request: {ex.Message}");
                return null;
            }
        }

        // ---------------------------------------------------------------------
        // Get all service requests
        public async Task<List<serviceTBL>> GetAllServicesAsync()
        {
            try
            {
                return await _context.Services
                    .OrderByDescending(s => s.CreatedUtc)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while retrieving services: {ex.Message}");
                return new List<serviceTBL>();
            }
        }

        // ---------------------------------------------------------------------
        // Get by ID (BST lookup built from current data)
        public async Task<serviceTBL?> GetByIdAsync(int id)
        {
            try
            {
                var all = await _context.Services.AsNoTracking().ToListAsync();
                var idx = SimpleRequestIndex.Build(all);
                return idx.TryGetById(id, out var req) ? req : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while fetching service by ID: {ex.Message}");
                return null;
            }
        }

        // ---------------------------------------------------------------------
        // Advance status
        public async Task<bool> AdvanceStatusAsync(int id, RequestStatus next)
        {
            try
            {
                var entity = await _context.Services.FirstOrDefaultAsync(s => s.ServiceID == id);
                if (entity is null) return false;

                entity.Status = next;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while updating status: {ex.Message}");
                return false;
            }
        }

        // ---------------------------------------------------------------------
        // Top urgent list (Max-Heap)
        public async Task<List<serviceTBL>> GetTopUrgentAsync(int count = 10)
        {
            try
            {
                var all = await _context.Services.AsNoTracking().ToListAsync();
                var idx = SimpleRequestIndex.Build(all);
                return idx.TopUrgent(count);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while retrieving top urgent: {ex.Message}");
                return new List<serviceTBL>();
            }
        }

        // ---------------------------------------------------------------------
        // Related requests (Graph BFS)
        public async Task<List<serviceTBL>> GetRelatedAsync(int id)
        {
            try
            {
                var all = await _context.Services.AsNoTracking().ToListAsync();
                var idx = SimpleRequestIndex.Build(all);
                var relatedIds = idx.RelatedIds(id).ToHashSet();
                return all.Where(s => relatedIds.Contains(s.ServiceID))
                          .OrderByDescending(s => s.Priority)
                          .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while retrieving related services: {ex.Message}");
                return new List<serviceTBL>();
            }
        }

        private sealed class SimpleRequestIndex
        {
            // ------------------ BST (by ServiceID) ------------------
            private sealed class BstNode
            {
                public int K; public serviceTBL V;
                public BstNode? L, R;
                public BstNode(int k, serviceTBL v) { K = k; V = v; }
            }
            private BstNode? _root;

            private void BstInsert(int k, serviceTBL v)
            {
                _root = Insert(_root, k, v);
                static BstNode Insert(BstNode? n, int k, serviceTBL v)
                {
                    if (n is null) return new BstNode(k, v);
                    if (k < n.K) n.L = Insert(n.L, k, v);
                    else if (k > n.K) n.R = Insert(n.R, k, v);
                    else n.V = v;
                    return n;
                }
            }

            public bool TryGetById(int k, out serviceTBL? v)
            {
                var n = _root;
                while (n is not null)
                {
                    if (k == n.K) { v = n.V; return true; }
                    n = k < n.K ? n.L : n.R;
                }
                v = null; return false;
            }

            // ------------------ Max-Heap (priority then recency) ------------------
            private sealed class MaxHeap
            {
                private readonly List<serviceTBL> _a = new();
                public int Count => _a.Count;

                public void Push(serviceTBL x)
                {
                    _a.Add(x);
                    for (int i = _a.Count - 1; i > 0;)
                    {
                        int p = (i - 1) / 2;
                        if (Cmp(_a[i], _a[p]) <= 0) break;
                        (_a[i], _a[p]) = (_a[p], _a[i]); i = p;
                    }
                }

                public serviceTBL Pop()
                {
                    var top = _a[0];
                    _a[0] = _a[^1];
                    _a.RemoveAt(_a.Count - 1);
                    for (int i = 0; ;)
                    {
                        int l = 2 * i + 1, r = l + 1, b = i;
                        if (l < _a.Count && Cmp(_a[l], _a[b]) > 0) b = l;
                        if (r < _a.Count && Cmp(_a[r], _a[b]) > 0) b = r;
                        if (b == i) break;
                        (_a[i], _a[b]) = (_a[b], _a[i]); i = b;
                    }
                    return top;
                }

                public IEnumerable<serviceTBL> PeekMany(int n)
                {
                    var temp = new List<serviceTBL>();
                    while (Count > 0 && temp.Count < n) temp.Add(Pop());
                    foreach (var x in temp) yield return x;   // emit
                    foreach (var x in temp) Push(x);          // restore
                }

                private static int Cmp(serviceTBL a, serviceTBL b)
                {
                    int p = a.Priority.CompareTo(b.Priority);     // higher priority first
                    if (p != 0) return p;
                    return a.CreatedUtc.CompareTo(b.CreatedUtc);  // newer first
                }
            }

            private readonly MaxHeap _heap = new();

            // ------------------ Graph (same-status edges) ------------------
            private readonly Dictionary<int, HashSet<int>> _adj = new();

            private void AddEdge(int a, int b)
            {
                (_adj.TryGetValue(a, out var sa) ? sa : _adj[a] = new()).Add(b);
                (_adj.TryGetValue(b, out var sb) ? sb : _adj[b] = new()).Add(a);
            }

            // ------------------ Build & Queries ------------------
            public static SimpleRequestIndex Build(IEnumerable<serviceTBL> all)
            {
                var idx = new SimpleRequestIndex();

                foreach (var s in all)
                {
                    idx.BstInsert(s.ServiceID, s);
                    idx._heap.Push(s);
                }

                // Simple "related" rule: connect items with the same Status.
                var byStatus = all.GroupBy(s => s.Status);
                foreach (var group in byStatus)
                {
                    var ids = group.Select(x => x.ServiceID).ToList();
                    for (int i = 0; i < ids.Count; i++)
                        for (int j = i + 1; j < ids.Count; j++)
                            idx.AddEdge(ids[i], ids[j]);
                }

                return idx;
            }

            public List<serviceTBL> TopUrgent(int count) => _heap.PeekMany(count).ToList();

            public List<int> RelatedIds(int startId, int max = 15)
            {
                var result = new List<int>();
                if (!_adj.ContainsKey(startId)) return result;

                var q = new Queue<int>();
                var seen = new HashSet<int> { startId };
                q.Enqueue(startId);

                while (q.Count > 0 && result.Count < max)
                {
                    var v = q.Dequeue();
                    foreach (var n in _adj[v])
                    {
                        if (seen.Add(n))
                        {
                            result.Add(n);
                            q.Enqueue(n);
                            if (result.Count >= max) break;
                        }
                    }
                }
                return result;
            }
        }
    }
}

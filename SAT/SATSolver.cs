﻿namespace SAT
{
    public static class SATSolver
    {
        public static string[] Solve(string path)
        {
            List<List<int>> clauses = ParseDIMACS(path);
            List<int> assignement = new();
            if (DPLL(clauses, assignement))
            {
                return new string[] { "SAT", string.Join(' ', assignement.Distinct()) };
            }
            else
            {
                return new string[] { "UNSAT" };
            }
        }

        public static bool DPLL(List<List<int>> clauses, List<int> assignment)
        {
            List<int> unitLiterals = clauses.Where(clause => clause.Count == 1).Select(c => c[0]).ToList();
            unitLiterals.ForEach(literal =>
            {
                clauses.RemoveAll(clause => clause.Contains(literal));
                clauses.ForEach(clause => clause.Remove(-literal));
            });
            assignment.AddRange(unitLiterals);

            var literals = clauses.SelectMany(c => c).Distinct().ToList();

            var pureLiterals = literals.GroupBy(l => Math.Abs(l)).Where(g => g.Count() == 1).SelectMany(g => g).ToList();

            pureLiterals.ForEach(literal => clauses.RemoveAll(clause => clause.Contains(literal)));
            assignment.AddRange(pureLiterals);

            if (clauses.Any(clause => clause.Count == 0))
            {
                pureLiterals.ForEach(pure => assignment.RemoveAll(x => pure == x));
                unitLiterals.ForEach(unit => assignment.RemoveAll(x => unit == x));
                return false;
            }

            if (clauses.Count == 0)
            {
                return true;
            }

            var chosenLiteral = clauses.SelectMany(c => c).FirstOrDefault();
            var clausesAddTrue = new List<List<int>>();
            clauses.ForEach(clause => clausesAddTrue.Add(new List<int>(clause)));
            clausesAddTrue.Add(new List<int> { -chosenLiteral });
            if (DPLL(clausesAddTrue, assignment))
            {
                assignment.Add(-chosenLiteral);
                return true;
            }

            assignment.RemoveAll(x => x == -chosenLiteral);
            var clausesAddFalse = new List<List<int>>();
            clauses.ForEach(clause => clausesAddFalse.Add(new List<int>(clause)));
            clausesAddFalse.Add(new List<int> { chosenLiteral });
            if (DPLL(clausesAddFalse, assignment))
            {
                assignment.Add(chosenLiteral);

                return true;
            }

            assignment.RemoveAll(x => x == chosenLiteral);
            pureLiterals.ForEach(pure => assignment.RemoveAll(x => pure == x));
            unitLiterals.ForEach(unit => assignment.RemoveAll(x => unit == x));
            return false;
        }

        private static List<List<int>> ParseDIMACS(string path)
        {
            List<List<int>> clauses;
            using (var streamReader = new StreamReader(path))
            {
                clauses = new List<List<int>>();
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line[0] == 'c') continue;
                    if (line[0] == 'p') continue;
                    clauses.Add(new List<int>(line
                        .Split(' ')
                        .Where(c => c != string.Empty && c != "0")
                        .Select(c => int.Parse(c))
                        .ToList()));
                }
            }
            return clauses;
        }
    }
}
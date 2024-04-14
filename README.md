# SAT_solver

#### Простой консольный SAT-решатель, использующий алгоритм DPLL и формат ввода/вывода DIMACS.

### Окружение:
`.Net 6`

### Сборка:
`dotnet build ./SAT/SAT.sln`

### Запуск тестов:
`dotnet test ./SAT/SAT.sln`

### Использование:
```c#
  var path = "..."; //путь к входному файлу в формате DIMACS
  string[] result = SATSolver.Start(path);
  // result[0] — SAT/UNSAT
  // result[1] — набор литералов, которые должны быть истины. Существует только для SAT 
```
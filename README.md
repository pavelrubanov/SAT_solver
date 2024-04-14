# SAT_solver

### Окружение:
`.Net 6`

### Сборка:
`dotnet build ./SAT/SAT.sln`

### Запуск тестов:
`dotnet test ./SAT/SAT.sln`

### Использование:
```c#
  var path = "...";
  string[] result = SAT.Start(path);
  // result[0] — SAT/UNSAT
  // result[1] — набор литералов, которые должны быть истины. Существует только для SAT 
```
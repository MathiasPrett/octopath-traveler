---
name: clean-code
description: Revisa código C# de este proyecto contra las reglas de Clean Code de IIC2113 (Capítulos 2 Nombres, 3 Funciones, 4 Herencia) que definen los 6 puntos de la nota de limpieza. Úsala antes de cerrar cualquier día de trabajo, después de escribir o modificar clases, o cuando el usuario pida revisar limpieza, nombres, tamaño de funciones o uso de herencia.
---

# Clean Code — IIC2113

Revisión de limpieza según los criterios con que se corrige este proyecto. La nota de limpieza parte en 6.0 y **baja por descuentos**, así que el objetivo es eliminar infracciones, no agregar sofisticación.

| Capítulo | Descuento máximo |
|---|---|
| 2 — Nombres | −2.0 |
| 3 — Funciones | −2.5 |
| 4 — Herencia | −1.5 |

La nota final es `sqrt(funcionalidad * limpieza) + 1`, así que perder limpieza arrastra la nota completa aunque los tests pasen.

## Cómo hacer la revisión

1. Determina el alcance: si el usuario no lo dice, revisa los archivos `.cs` modificados según `git status` / `git diff`. Ignora `bin/`, `obj/` y `Program.cs` (viene dado por el curso).
2. Recorre cada archivo aplicando los tres capítulos de abajo.
3. Reporta las infracciones **agrupadas por capítulo**, cada una con `archivo:línea`, el problema en una frase y la corrección concreta.
4. Ordena por gravedad: primero lo que descuenta más.
5. Si no hay infracciones en un capítulo, dilo explícitamente en vez de omitirlo.
6. **No apliques los cambios salvo que el usuario lo pida.** Primero reporta.

Al final, entrega una estimación honesta del descuento por capítulo. Si algo es discutible, márcalo como discutible en vez de inflar el reporte.

---

## Capítulo 2 — Nombres (−2.0)

**Revisar:**
- Nombres que revelen intención: `CalculateDamage()`, no `Calc()` ni `Process()` ni `Handle()` ni `DoThing()`.
- Clases = sustantivos (`Unit`, `TurnQueue`, `CombatEngine`). Métodos = verbos (`LoadTeams()`, `ValidateTeam()`, `ExecuteAttack()`).
- Métodos que devuelven `bool` se leen como afirmación: `IsValid`, `HasRepeatedNames`, `AreActiveSkillsValid`.
- **Una palabra por concepto**: no mezclar `Get*` / `Retrieve*` / `Fetch*` para lo mismo. En este proyecto la convención ya establecida es `Load*` (leer de disco), `Find*` (buscar en memoria, devuelve `null` si no está), `Require*` (buscar y lanzar si no está), `Build*` (construir objetos de dominio), `Parse*` (texto → estructura).
- Sin desinformación: `playerTeam`, no `teamData`.
- Consistencia de mayúsculas: PascalCase en clases/métodos/campos públicos, `_camelCase` en campos privados, `camelCase` en locales y parámetros.
- **Sin números ni strings mágicos**: todo literal con significado va a una constante con nombre. Ya existen ejemplos en el repo: `NotFound = -1`, `MaxActiveSkills = 8`, `PlayerTeamHeader = "Player Team"`.

**Ojo en este proyecto:** los literales `1.3` (modificador de daño), `40` (largo del separador), `5` (tope de BP), `"----..."`, y cada string de output exacto son candidatos obligados a constante.

## Capítulo 3 — Funciones (−2.5)

Es el capítulo que más descuenta. Revisar cada método:

- **Largo: 4-5 líneas de cuerpo.** Sobre 10 es infracción clara.
- **Una sola responsabilidad.** Si al describir el método necesitas una "y", son dos métodos.
- **Máximo 2 niveles de indentación.** Un tercer nivel (`if` dentro de `for` dentro de `if`) significa extraer un método.
- **Máximo 3 argumentos**, idealmente 1-2. Si necesitas más, agrupa en un objeto (en este repo, `GameCatalog` existe justamente para no pasar 4 listas sueltas).
- **Sin argumentos booleanos de bandera**: `ExecuteAttack(true)` → dos métodos, `ExecuteBasicAttack()` y `ExecuteSkillAttack()`.
- **Sin output arguments**: no pasar un objeto para que el método lo modifique por dentro; devuelve el resultado.
- **Un solo nivel de abstracción por función**: no mezclar la orquestación (`if (!IsValid) ShowError()`) con el detalle (`line.Substring(i+1, j-i-1)`) en el mismo cuerpo.
- **Sin código duplicado**: si algo aparece 2+ veces, extraer. Los constructores de clases de datos son la excepción razonable al límite de argumentos.

**Excepción aceptada:** los constructores de modelos (`Traveler`, `Beast`, `Stats`) reciben todos sus campos y eso está bien; el límite de 3 argumentos apunta a funciones de comportamiento.

## Capítulo 4 — Herencia (−1.5)

- Clase base con **lo genuinamente común**; lo específico va en las subclases. Aquí: `Unit` tiene `Name`/`Stats`/`Alive`; `Traveler` agrega SP/BP/armas/habilidades; `Beast` agrega skill/shields/debilidades.
- **No heredar sin aportar nada.** Si una subclase no agrega ni sobrescribe, no debería existir.
- **Sin herencia profunda**: máximo 2 niveles.
- Constructor de la base `protected` cuando la clase es `abstract`; la subclase llama a `base(...)`.
- `protected` para lo que las subclases necesitan, `private` para lo demás; no dejar todo `public` por comodidad.
- Métodos `virtual` en la base solo cuando alguna subclase realmente los sobrescribe. Declarar `virtual` "por si acaso" es ruido.
- **No mezclar herencia con interfaces** sin una razón explicable.
- **No duplicar en las subclases** un campo o lógica que podría vivir en la base.

---

## Descuentos frecuentes en este proyecto específico

- Métodos que mezclan parseo + validación + construcción en el mismo cuerpo (por eso `TeamLoader`, `TeamValidator` y `TeamBuilder` están separados — mantener esa separación).
- Escribir el output con strings literales repetidos en vez de constantes o un método de formato reutilizado.
- Un `CombatEngine` que crece hasta ser un método gigante con todo el loop de rondas adentro.
- Bucles anidados para buscar duplicados en vez de `HashSet`.
- Meter lógica de negocio en `Game.cs`, que debería solo orquestar.

## Formato del reporte

```
## Capítulo 2 — Nombres
- `Combat/CombatEngine.cs:42` — `Process()` no dice qué hace → `ExecuteTravelerTurn()`
- `Combat/DamageCalculator.cs:10` — literal `1.3` sin nombre → constante `BasicAttackModifier`

## Capítulo 3 — Funciones
- `Combat/CombatEngine.cs:55-78` — 24 líneas y 3 niveles de indentación; hace loop de ronda, turno y chequeo de fin → extraer `PlayRound()`, `PlayTurn()`, `IsCombatOver()`

## Capítulo 4 — Herencia
Sin infracciones.

## Descuento estimado
Cap. 2: −0.3 · Cap. 3: −0.8 · Cap. 4: 0.0
```

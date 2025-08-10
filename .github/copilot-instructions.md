# GitHub Copilot Instructions for Safely Hidden Away (Continued)

## Mod Overview and Purpose

**Safely Hidden Away (Continued)** is an updated version of the mod originally created by Uuugggg. This mod aims to enhance the gameplay experience by adjusting raid frequencies based on the remoteness of a player's base location. Bases situated in isolated or hard-to-reach areas, such as islands, experience fewer raids due to increased travel time. It also adjusts caravan and visitor frequencies similarly. The mod introduces a configurable system for delay times, enabling users to tailor the experience to their liking. High base wealth is a factor that can negate these delays to encourage player balance.

## Key Features and Systems

- **Remote Base Mechanics**: The remoteness of a base directly influences how often raids occur. Effective travel time to islands is considered through the coast proximity.
- **Configurable Delays**: The mod provides adjustable delay times through the in-game settings, allowing players to visualize changes via a graph.
- **Compatibility and Debugging**: Interfaces with development mode to allow viewing and resetting of raid timers, although caution is advised against using the "Future Incident" button.
- **Compatibility**: The mod works with old saves and can be removed without severe consequences, though it may result in minor errors related to raid timers.

## Coding Patterns and Conventions

- **Static Classes and Methods**: Utilized for utility operations in classes such as `TDWidgets`, `IncidentCategoryDefOf`, and `VistorDelayer`.
- **Inheritance**: Utilizes inheritance for custom RimWorld classes, for example, `Settings` inherits from `ModSettings`.
- **Consistency**: All public classes and methods are properly capitalized following C# naming conventions.
  
## XML Integration

- **Defining Content**: XML files are used to define adjustable parameters that correlate with the mod’s mechanics, ensuring interactions are consistent with RimWorld's content structure.

## Harmony Patching

- **Purpose**: Harmony is used for method patching, allowing safe extension and modification of the vanilla game code.
- **Application**: Focus on patching methods that calculate raid frequencies and visitor occurrences, aligned with the remote base mechanics.

## Suggestions for Copilot

- **Code Expansion**: Leverage Copilot to identify additional opportunities for optimizing the remoteness calculations and integrating more sophisticated delay algorithms.
- **Refactoring Guidance**: Request assistance to refactor existing static method classes to improve code readability and maintainability, such as consolidating utility methods where applicable.
- **Edge Case Handling**: Use Copilot to explore and manage potential edge cases, particularly focusing on high-wealth scenarios canceling delays or ensuring compatibility with other mods.
- **Debugging Enhancements**: Suggest enhancements for debugging tools and output logs, particularly in the `DebugLog.cs` to ensure comprehensive debugs and user-friendly error messages.

This .github/copilot-instructions.md file should serve as a comprehensive guide to assist in further development and maintenance of the "Safely Hidden Away (Continued)" mod.

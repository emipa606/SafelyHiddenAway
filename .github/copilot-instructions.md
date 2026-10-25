# GitHub Copilot Instructions for SafelyHiddenAway (Continued) Mod

## Mod Overview and Purpose

SafelyHiddenAway (Continued) is a RimWorld mod that updates the Safely Hidden Away mod created by Uuuggggs, who has passed away. The mod's primary purpose is to reduce the frequency of raids and caravans visiting remote bases, with the delay time being configurable. The delay is determined by the nearest coast to the base, plus a few days. High wealth can also offset this delay. The mod works with old saves and can be removed, although resetting the raid timer will result in an error.

## Key Features and Systems

- Configurable delay times for raids and caravans visiting remote bases.
- Remote bases have a reduced chance of being raided due to longer travel times.
- High wealth can offset the delay.
- Dev mode provides the ability to view and reset the raid timer.
- Compatibility with old saves and the ability to be removed.

## Coding Patterns and Conventions

- The mod uses the Harmony patching library to modify game behavior.
- The code is organized into separate C# files, each containing a specific type or set of related types.
- XML files are used to define mod metadata and dependencies.
- Code is written in C# and follows the conventions of the RimWorld modding API.

## XML Integration

The mod includes an `About.xml` file that defines the mod's metadata and dependencies. The file includes the following elements:

- `name`: The name of the mod.
- `author`: The author of the mod.
- `packageId`: The unique identifier for the mod.
- `version`: The version of the mod.
- `dependencies`: A list of dependencies required by the mod.

## Harmony Patching

The mod uses the Harmony patching library to modify game behavior. Specifically, it patches the following methods:

- `TryFindClosestTile`: This method is used to find the nearest coast to a base, which is used to determine the travel time for raids and caravans.
- `DelayDays`: This method is used to calculate the delay time for raids and caravans based on the base's location and wealth.

The patches are implemented in the `DelayDays.cs` file, which contains the following types:

- `DelayDays`: A class that calculates the delay time for raids and caravans.
- `IncidentCategoryDefOf`: A class that defines the category of incidents that can occur in the game.
- `SitePartDefOf`: A class that defines the parts of a site that can be used in the game.

## Suggestions for Copilot

Here are some suggestions for using GitHub Copilot with the SafelyHiddenAway (Continued) mod:

- Use Copilot to generate code snippets for new features or bug fixes.
- Use Copilot to generate code for new types or methods.
- Use Copilot to generate XML definitions for new features or settings.
- Use Copilot to generate Harmony patches for modifying game behavior.
- Use Copilot to generate documentation for new features or settings.

By following these guidelines, you can use GitHub Copilot to enhance your productivity when working on the SafelyHiddenAway (Continued) mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.


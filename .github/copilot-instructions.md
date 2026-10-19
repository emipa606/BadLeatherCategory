# GitHub Copilot Instructions for Bad Leather Category Mod

## Mod Overview and Purpose

**Mod Name:** Bad Leather Category  
**Author:** Mlie  
**Purpose:** The "Bad Leather Category" mod for RimWorld addresses the need to categorize leathers sourced from Humanlike races, which would otherwise evoke negative thoughts when worn. The mod achieves this by creating a distinct category for such leathers, offering a more intuitive item management experience in-game.

## Key Features and Systems

- **Categorization of Humanlike Leathers:** Automatically sorts leathers derived from Humanlike races into a specific category to prevent negative thought debuffs from their use.
- **User-Friendly Display:** Clear labeling and categorization make it easy for players to manage leathers in their inventories.

## Coding Patterns and Conventions

- **C#:** The mod features a single C# file, `BadLeatherCategory.cs`, which contains the core logic. The code adheres to standard C# conventions and practices, ensuring readability and maintainability.
    - **Naming Conventions:** Use of PascalCase for class names and method names, and camelCase for local variables.
    - **Commenting:** Inline comments are used to explain key sections of the code, especially where complex logic is implemented.

## XML Integration

- **XML File:** The mod uses an XML file, `ThingCategories.xml`, located in the `TranslationTemplate/ThingCategoryDef` directory. This file defines the new category for the specific leathers.
- **Integration with RimWorld:** The XML structure is compliant with RimWorld’s modding XML schema, ensuring seamless integration and recognition by the game.

## Harmony Patching

- **Usage:** Harmony is utilized for patching the game's original methods to incorporate the new category without altering the base game files directly.
- **Best Practices:** 
    - Use Harmony's prefix and postfix methods to adjust game behavior.
    - Ensure any patches are robust and do not affect the game's stability.

## Suggestions for Copilot

- **Autocomplete Suggestions:** Ensure Copilot autocompletes reflect the project's naming conventions and coding patterns, especially class names and methods related to the new category.
- **Contextual Understanding:** When working within `BadLeatherCategory.cs`, Copilot should suggest code enhancers that improve efficiency in sorting and categorizing items.
- **XML Suggestions:** For `ThingCategories.xml`, suggestions should align with RimWorld's XML structure, ensuring accurate tagging and attribute usage.
- **Harmony Patching:** Provide contextual suggestions for creating Harmony patch methods, particularly focusing on prefix and postfix techniques.

By following these guidelines and utilizing GitHub Copilot's AI-driven code assistant capabilities, developers can efficiently expand and maintain the "Bad Leather Category" mod with code suggestions that adhere to the project's established patterns and conventions.

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


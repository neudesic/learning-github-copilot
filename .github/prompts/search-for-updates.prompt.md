
# search-for-updates Workflow Prompt

## Objective
Identify, compare, and prioritize new features or updates from GitHub Copilot and VS Code documentation, aligning them with curriculum needs and gaps.

## Steps
1. Search official documentation sources for recent updates:
   - GitHub Copilot: https://github.com/github/docs/tree/main/content/copilot
   - VS Code: https://code.visualstudio.com/updates
2. List new features, changes, or deprecations with brief descriptions and source links.
3. For each item, use sequential thinking to compare with current curriculum content (refer to curriculum-map, target-audience, and curriculum-gaps files).
4. Assess if new information modifies the approach for the target audience.
5. Add extra weight to updates that address or improve identified curriculum gaps.
6. Draft a summary of recommended changes in `.curriculum-admin/proposed-changes.md` for user review before creating any GitHub issues.
7. Only create GitHub issues for curriculum updates after user approval of proposed changes.

## Tools
- GitHub repo tool
- Documentation search
- Sequential thinking tool

## Output
- Table of new/updated features with links and descriptions
- Summary of recommended curriculum changes (proposed-changes.md)
- GitHub issues (after user review)

## Example Prompt
"Search the official documentation sources for VS Code and GitHub Copilot. List all new features or updates from the past month, including a brief description and source link for each. Compare each update to the current curriculum (using curriculum-map, target-audience, and curriculum-gaps), and summarize recommended changes in .curriculum-admin/proposed-changes.md for user review before creating any GitHub issues."

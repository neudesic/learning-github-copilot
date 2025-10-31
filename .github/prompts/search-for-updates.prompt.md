---
mode: 'agent'
description: 'Identify and summarize new features or updates from GitHub documentation repos (VS Code, GitHub Copilot)'
tools: ['sequentialthinking','editFiles','changes', 'codebase', 'fetch', 'githubRepo', 'problems', 'runCommands', 'runNotebooks', 'search', 'usages', 'microsoft-docs', 'add_comment_to_pending_review', 'add_issue_comment', 'create_and_submit_pull_request_review', 'create_branch', 'create_issue', 'create_pull_request', 'get_commit', 'get_discussion', 'get_discussion_comments', 'get_file_contents', 'get_issue', 'get_issue_comments', 'get_pull_request', 'get_pull_request_comments', 'get_pull_request_diff', 'get_pull_request_files', 'get_pull_request_reviews', 'get_pull_request_status', 'get_tag', 'list_branches', 'list_issues', 'list_tags', 'search_code', 'search_issues', 'search_repositories', 'update_issue']
---

# search-for-updates Workflow Prompt

## Objective
Identify, compare, and prioritize new features or updates from GitHub Copilot and VS Code documentation, aligning them with curriculum needs and gaps.

### Required outputs
.curriculum-admin/proposed-changes.md - list of proposed changes to make to the curriculum for user review before github issue creation.


## Steps
1. Search official documentation sources for recent updates:
   - GitHub Copilot: https://github.com/github/docs/tree/main/content/copilot
   - VS Code: https://code.visualstudio.com/updates
2. List new features, changes, or deprecations with brief descriptions and source links.
3. For each item, use sequential thinking to compare with current curriculum content (refer to curriculum-map, target-audience, and curriculum-gaps files). Ensure these columns/properties exist for the update:
- Date of the recommendation (today's date)
- Update
- Brief Description
- Source Link
- Recommendation analysis (reason for inclusion)
- Priority (High/Medium/Low)
- Gap analysis (if it addresses current identified gaps in content)
- User impact (how it improves the user experience for the target-audience)
4. Assess if new information modifies the approach for the target audience.
5. Add extra weight to updates that address or improve identified curriculum gaps.
6. Draft a summary of recommended changes in `.curriculum-admin/proposed-changes.md` for user review before creating any GitHub issues.
7. Only create GitHub issues for curriculum updates after user approval of proposed changes.
8. Remove any proposed changes from the proposed-changes.md ONLY after the github issue is confirmed to exist.


## Output
- Table of new/updated features with links and descriptions in the .curriculum-admin/proposed-changes.md
- Summary of recommended curriculum changes (proposed-changes.md) output to chat window
- GitHub issues (after user review) created by referencing the proposed-changes.md (it can be updated by the user after initial creation)

## Example Prompt
"Search the official documentation sources for VS Code and GitHub Copilot. List all new features or updates from the past month, including a brief description and source link for each. Compare each update to the current curriculum (using curriculum-map, target-audience, and curriculum-gaps), and summarize recommended changes in .curriculum-admin/proposed-changes.md for user review before creating any GitHub issues."


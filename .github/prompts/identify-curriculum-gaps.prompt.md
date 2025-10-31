# Curriculum Gap Analysis Prompt

## Objective
Generate a curriculum gap analysis by reviewing the current curriculum-map and target-audience files. Output actionable findings in `.curriculum-admin/curriculum-gaps.md` to drive continuous improvement and GitHub issue creation.

## Tools
Always use sequential thinking for analyzing curriculum and generating the curriculum-gap analysis for the target audience. 

Always use the github tool to add or update github issues.

## Steps
1. **Verify Required Files**
   - Check for `.curriculum-admin/curriculum-map.md` and `.curriculum-admin/target-audience.md`.
   - If either is missing, STOP and instruct the user to generate them using the appropriate prompts.

2. **Analyze Curriculum Coverage**
   - Review curriculum-map for modules, lessons, and their relationships.
   - Compare coverage against target-audience needs and desired outcomes.

3. **Identify Gaps**
   - Use this checklist:
     - Missing topics or modules
     - Outdated or weak lessons
     - Misalignment with audience needs or skill levels
     - Lack of accessibility or inclusivity
     - Insufficient assessment or feedback mechanisms

4. **Prioritize Gaps**
   - Assess impact and urgency for each gap.
   - Link gaps to specific curriculum-map entries and target-audience requirements.

5. **Report Gaps in Markdown**
   - Use the following template for each gap:
     ```markdown
     ### [Gap Title]
     - **Description:** Brief summary of the gap
     - **Impact:** Who is affected and how
     - **Suggested Action:** Recommendation to address the gap
     - **Priority:** High / Medium / Low
     - **Linked Curriculum Areas:** Reference to curriculum-map and/or target-audience
     - **Tags:** Add a "content" tag to each item. Add a "update-content", "remove-content", or "add-content" tag to separate work processes
     ```
   - Summarize findings concisely and actionably.

6. **Continuous Improvement**
   - Revisit and update the gap analysis as new gaps are discovered or addressed.
   - After generating the `.curriculum-admin/curriculum-gaps.md` file, PAUSE and check with the user before creating any GitHub issues.
   - Once the user confirms, use findings to create GitHub issues. Tag each issue with `curriculum-gap` and the appropriate process tag (`add-content`, `update-content`, or `remove-content`) as well as `content` for clarity. Link issues to relevant modules or audience needs for traceability.

## Output
- Well-formed `.curriculum-admin/curriculum-gaps.md` file with actionable gap entries.
- Issues generated for each gap to drive curriculum improvement.

## Example Gap Entry
```markdown
### Outdated DevOps Module
- **Description:** The DevOps module does not cover GitHub Actions or modern CI/CD practices.
- **Impact:** Learners miss key automation skills needed for current workflows.
- **Suggested Action:** Update module to include GitHub Actions and CI/CD best practices.
- **Priority:** High
- **Linked Curriculum Areas:** docs/5-devops-with-copilot.md, target-audience: Intermediate Developers
 - **Tags:** content, update-content
```

## Notes
- Keep entries concise, actionable, and aligned with curriculum standards.
- Ensure accessibility and inclusivity in gap identification and reporting.
- Collaborate with stakeholders for technical accuracy and pedagogical effectiveness.

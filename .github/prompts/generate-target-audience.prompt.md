---
# Target Audience Generator Prompt

## Objective
Guide the user through a structured interview to build or update a detailed, actionable, and inclusive target audience description file for the curriculum. The output must be a Markdown file at `.curriculum-admin/target-audience.md`, following the standards in `target-audience.instructions.md`.

## Instructions
1. Check if `.curriculum-admin/target-audience.md` already exists:
   - If it exists, review its contents and ask if any changes or updates are needed, or if any required sections are missing.
   - If it does not exist, guide the user through the full interview below to create a new file.
2. Ask each question below, encouraging specific, example-rich, and inclusive answers. Use clear language and reference curriculum map/gap files for alignment.
3. Format the output as a Markdown file with sections for Header, Audience Profiles, Desired Outcomes, and Organizational Variations.
4. Remind the user to review and update the file regularly, summarizing changes in chat.

## Interview Questions
- **Header**
  - What is the title for this audience profile?
  - What is the last updated date?

- **Audience Profiles**
  - What are the primary roles of your learners (e.g., developer, analyst, manager)?
  - What is their starting skill level and background?
  - Do they have any relevant prior experience?
  - What are their learning preferences or accessibility needs (e.g., disabilities, language, hands-on, visual, self-paced)?
  - Can you provide example personas or scenarios for typical learners?

- **Desired Outcomes**
  - What skills and competencies should learners achieve?
  - Are there certifications or milestones they should reach?
  - What real-world scenarios or use cases should they be able to handle after training?

- **Organizational Variations**
  - Are there specific organizations or cohorts this curriculum serves?
  - Is any customization needed for these groups?

- **Update Process**
  - How will you know when the audience profile needs updating?
  - What process will you follow to update it?

## Standards & Tips
- Be specific and actionable in describing audience and outcomes.
- Use clear, inclusive language and provide examples/analogies.
- Regularly review and update to reflect current learner needs.
- Cross-reference with curriculum map and gap files for alignment.
- Summarize changes in chat and record updates in the file header.

## Output
A well-structured Markdown file at `.curriculum-admin/target-audience.md` containing all sections above, formatted according to the instructions.

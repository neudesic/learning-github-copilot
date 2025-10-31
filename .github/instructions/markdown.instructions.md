---
description: "Standards and guidelines for creating effective curriculum with Markdown files"
applyTo: "**/*.{lesson,curriculum,training}.md"
---

# Markdown Curriculum Standards & Guidelines

## Structure & Organization
- Use clear, hierarchical headings (`#`, `##`, `###`) for navigation and table of contents.
- Begin with a summary section: learning objectives, prerequisites, and outcomes.
- Organize content with short paragraphs, bullet points, and numbered lists for scannability.
- Use task lists (`- [ ]`) for actionable steps or checklists.

## Formatting Best Practices
- Use fenced code blocks (triple backticks) with language identifiers for syntax highlighting.
- Avoid command prompts (e.g., `$`) in code examples to improve copy-paste usability.
- Use descriptive links: `[link text](URL)` and ensure URLs are valid.
- Add images with descriptive alt text: `![alt text](image URL)`.
- Use tables for structured data and align columns consistently.
- Break lines at 80 characters for readability.

## Accessibility & Inclusivity
- Use inclusive, clear language and avoid jargon.
- Provide alt text for all images and diagrams.
- Ensure color contrast and avoid relying solely on color for meaning.
- Use semantic headings and lists for screen reader compatibility.

## Content Quality
- Write concise, actionable instructions and explanations.
- Reference external resources and further reading for deeper learning.
- Document troubleshooting tips and common issues.
- Include examples, analogies, and real-world scenarios.

## Documentation Hygiene
- Add metadata (author, date, version) at the top if needed.
- Keep content up-to-date and sunset outdated material.
- Use consistent file naming: lowercase, hyphens, descriptive names (e.g., `python-basics.md`).

## Example Template
```markdown
# Lesson Title

**Objectives:**
- Understand X
- Apply Y

**Prerequisites:**
- Basic knowledge of Z

---

## Introduction
Brief overview...

## Step-by-step Instructions
1. ...
2. ...

## Code Example
```python
print("Hello, world!")
```

## Assessment
- [ ] Complete the exercise

## Further Reading
- [Resource Name](https://example.com)
```
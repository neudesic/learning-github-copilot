---
description: "Standards and guidelines for creating effective curriculum with Jupyter Notebooks"
applyTo: "**/*.{lesson,curriculum,training}.ipynb"
---

# Jupyter Notebook Curriculum Standards & Guidelines

## Structure & Organization
- Start with a markdown cell for lesson title, objectives, author, date, and version.
- Use markdown cells for explanations, instructions, and summaries.
- Use code cells for hands-on exercises, demonstrations, and assessments.
- Organize content into logical sections: introduction, activities, assessment, summary.

## Formatting Best Practices
- Add comments and docstrings in code cells for clarity.
- Provide sample outputs and troubleshooting tips.
- Use images, tables, and links to enrich content.
- Use headings and lists in markdown cells for navigation and scannability.

## Accessibility & Inclusivity
- Use clear, inclusive language and avoid jargon.
- Provide alt text for all images and diagrams.
- Ensure logical cell order and semantic structure for screen readers.

## Content Quality
- Write concise, actionable instructions and explanations.
- Reference external resources and further reading for deeper learning.
- Include real-world examples and analogies.

## Documentation Hygiene
- Document metadata (author, date, version) in the first cell.
- Keep notebooks up-to-date and sunset outdated material.
- Use consistent file naming: lowercase, hyphens, descriptive names (e.g., `data-visualization.ipynb`).

## Example Template
```json
{
  "cells": [
    {
      "cell_type": "markdown",
      "metadata": {"language": "markdown"},
      "source": [
        "# Lesson Title\n",
        "**Objectives:**\n",
        "- Understand X\n",
        "- Apply Y\n",
        "**Author:** Name\n",
        "**Date:** YYYY-MM-DD\n",
        "**Version:** 1.0\n"
      ]
    },
    {
      "cell_type": "markdown",
      "metadata": {"language": "markdown"},
      "source": [
        "## Introduction\n",
        "Brief overview..."
      ]
    },
    {
      "cell_type": "code",
      "metadata": {"language": "python"},
      "source": [
        "# Example code cell\n",
        "print('Hello, world!')"
      ]
    },
    {
      "cell_type": "markdown",
      "metadata": {"language": "markdown"},
      "source": [
        "## Assessment\n",
        "- Complete the exercise"
      ]
    },
    {
      "cell_type": "markdown",
      "metadata": {"language": "markdown"},
      "source": [
        "## Further Reading\n",
        "- [Resource Name](https://example.com)"
      ]
    }
  ]
}
```
# Copilot Instructions for This Repository

## Project Overview
This repository is designed for Copilot training, providing hands-on materials, prompts, and configuration files to help contributors and Copilot agents learn, test, and improve their use of GitHub Copilot and related AI tools.

## Folder Structure Summary
- `docs/` — Main training lessons and documentation for Copilot usage and best practices.
- `samples/` — Sample applications (e.g., eShop, Orders, SimpleFullStack) for practical exercises and demonstrations.
- `_schedule/` — Outlines and lesson plans for structured Copilot training sessions.
- `package.json`, `README.md` — Project configuration and overview.
- `.curriculum-admin/` — Curriculum administration metadata. Contains:
  - `curriculum-map.md`: Overview of modules, lessons, and their relationships.
  - `target-audience.md`: Defines learner profiles, starting skill levels, and desired outcomes.
  - `curriculum-gaps.md`: Identifies missing, weak, or outdated areas in the curriculum.
  These files are used by Copilot and LLMs to provide rich context for training content analysis, updates, and understanding the learner perspective. They enable more efficient and targeted curriculum improvements by making gaps, audience needs, and curriculum structure explicit.

## Copilot Agents & Chatmodes
- **Curriculum Designer Chatmode**: Assists in designing, reviewing, and updating curriculum content. Provides recommendations for lesson structure, learning outcomes, and gap analysis based on `.curriculum-admin` files.
- **C# Agent**: Enforces C# coding standards, style, and best practices. Use for C# code reviews and generation.
- **Azure Bicep Assistant**: Specializes in Azure IaC, Bicep, and ARM best practices. Use for infrastructure code and reviews.
- **Security Review Agent**: Focuses on secure coding, vulnerability detection, and API security. Use for security audits and feedback.
- **Default Copilot/Chat**: General-purpose Copilot for code suggestions, documentation, and Q&A.


## Main Contributor/Agent Tasks
- Review and improve code using Copilot agents and provided prompts.
- Complete training exercises and lessons in `docs/` and `_schedule/`.
- Generate, review, or update sample code in `samples/` as part of training.
- Follow conventions and best practices outlined in `.github/prompts/`.
- Contribute new prompts, lessons, or sample scenarios as needed.

## Intended Audience & Guidelines
- **Audience**: Developers, trainers, and Copilot agents seeking to learn or teach effective Copilot usage.
- **Guidelines**: Keep contributions concise, actionable, and aligned with the provided prompts and standards. Avoid unnecessary detail to ensure clarity and minimize token usage.

## Use of `.curriculum-admin` by Copilot & LLMs

When performing training content analysis, updates, or reviewing curriculum relevance, Copilot agents and LLMs use the `.curriculum-admin` files to:
- Understand the current curriculum structure and coverage (`curriculum-map.md`).
- Align content and recommendations with learner needs and desired outcomes (`target-audience.md`).
- Identify and prioritize gaps for improvement (`curriculum-gaps.md`).

These files provide essential context for delivering high-quality, learner-centered training and help minimize unnecessary token usage by focusing analysis and updates on what matters most.

Refer to the `README.md` and lesson files for more detailed instructions on specific training modules or sample applications.

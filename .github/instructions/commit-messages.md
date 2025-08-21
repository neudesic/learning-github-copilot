---
applyTo:
  - commitMessageGeneration
---

# Commit Message Instructions

## Format Structure

Use the **Conventional Commits** format for all commit messages:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

## Commit Types

- **feat**: A new feature for the user
- **fix**: A bug fix for the user
- **docs**: Documentation changes only
- **style**: Code style changes (formatting, missing semi-colons, etc.)
- **refactor**: Code changes that neither fix bugs nor add features
- **perf**: Performance improvements
- **test**: Adding or updating tests
- **chore**: Maintenance tasks, dependency updates
- **ci**: Changes to CI/CD configuration
- **build**: Changes to build system or external dependencies

## Scope Guidelines

Use descriptive scopes to indicate the area of change:
- **components**: React component changes
- **api**: Backend API changes
- **auth**: Authentication/authorization
- **ui**: User interface updates
- **docs**: Documentation updates
- **config**: Configuration changes

## Description Rules

- **Use imperative mood** ("add feature" not "added feature")
- **Limit to 50 characters** for the subject line
- **Start with lowercase** unless it's a proper noun
- **No period** at the end of the subject line
- **Be specific and descriptive** about what changed

## Body Guidelines

- **Wrap at 72 characters** per line
- **Explain what and why** not how
- **Include motivation** for the change
- **Reference related issues** using #issue-number
- **Separate paragraphs** with blank lines

## Footer Usage

- **Breaking changes**: Start with "BREAKING CHANGE: "
- **Issue references**: "Closes #123", "Fixes #456"
- **Co-authored by**: For pair programming

## Examples

```
feat(auth): add OAuth2 login integration

Implement Google OAuth2 authentication to allow users to login
using their Google accounts. This provides a more secure and
convenient authentication method.

- Add OAuth2 configuration
- Implement callback handling
- Update user model to store OAuth tokens

Closes #142
```

```
fix(components): resolve memory leak in UserProfile component

The UserProfile component was not properly cleaning up event
listeners in useEffect, causing memory leaks on route changes.

BREAKING CHANGE: UserProfile now requires cleanup prop
``` 
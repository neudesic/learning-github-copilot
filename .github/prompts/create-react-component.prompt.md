---
mode: 'agent'
model: Claude Sonnet 4
description: 'Create a new React component with TypeScript and tests'
tools: ['terminal']
---

# Create React Component

Create a new React component named **${input:componentName:Component name}** with the following requirements:

## Component Structure
- Use **TypeScript** with strict typing and proper interfaces
- Implement as a **functional component** with hooks
- Include **proper prop validation** with TypeScript interfaces
- Add **JSDoc documentation** for the component and its props
- Use **kebab-case** for the folder and file names

## Styling
- Include **CSS Modules** for component-specific styling
- Implement **responsive design** patterns
- Follow **WCAG 2.1 AA** accessibility standards
- Use **semantic HTML** elements

## Testing
- Create **comprehensive unit tests** using React Testing Library
- Include **accessibility tests** with jest-axe
- Test **user interactions** and **state changes**
- Mock any **external dependencies**

## File Organization
Create the component in: `src/components/${input:componentName}/`

### Required files:
- `${input:componentName}.tsx` - Main component file
- `${input:componentName}.module.css` - Component styles
- `${input:componentName}.test.tsx` - Unit tests
- `index.ts` - Export file
- `README.md` - Component documentation

## Additional Context
Component purpose: ${input:purpose:Brief description of what this component does}

Reference the [copilot-instructions.md](../copilot-instructions.md) for coding standards and best practices. 
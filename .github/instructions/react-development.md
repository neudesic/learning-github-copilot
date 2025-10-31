---
applyTo:
  - codeGeneration
  - testGeneration
  - reviewSelection
filePatterns:
  - "**/*.tsx"
  - "**/*.ts"
  - "**/*.jsx"
  - "**/*.js"
keywords:
  - react
  - component
  - hook
  - jsx
  - tsx
---

# React Development Instructions

## Component Architecture

- **Always use functional components** with hooks instead of class components
- **Create custom hooks** for reusable stateful logic across components
- **Use React.memo** for components that receive the same props frequently
- **Implement proper prop drilling solutions** with Context API or state management
- **Keep components small and focused** on a single responsibility

## TypeScript Integration

- **Define explicit interfaces** for all component props
- **Use generic types** for reusable components where appropriate
- **Avoid using `any` type** - prefer `unknown` or proper type definitions
- **Create union types** for component variants and states
- **Use type guards** for runtime type checking when needed

## State Management

- **Use useState** for simple local component state
- **Use useReducer** for complex state logic with multiple sub-values
- **Implement Context API** for application-wide state that multiple components need
- **Consider external state management** (Redux, Zustand) for complex applications
- **Keep state as close to where it's used** as possible

## Performance Optimization

- **Use useMemo** for expensive calculations that depend on specific props/state
- **Use useCallback** for functions passed as props to prevent unnecessary re-renders
- **Implement code splitting** with React.lazy and Suspense for large components
- **Optimize re-renders** by avoiding object/array creation in render methods
- **Profile performance** using React DevTools Profiler

## Error Handling

- **Implement Error Boundaries** for graceful error handling in component trees
- **Use try-catch blocks** in async functions and event handlers
- **Provide fallback UI** for error states and loading states
- **Log errors appropriately** without exposing sensitive information
- **Handle network errors** gracefully with retry mechanisms

## Testing Standards

- **Test component behavior** rather than implementation details
- **Use React Testing Library** for component testing
- **Mock external dependencies** and API calls
- **Test accessibility** with jest-axe
- **Maintain test coverage** above 80% for critical components

## Styling Guidelines

- **Use CSS Modules** for component-scoped styling
- **Follow BEM methodology** when using regular CSS
- **Implement design tokens** for consistent spacing, colors, and typography
- **Ensure responsive design** with mobile-first approach
- **Test across different devices** and browsers

## Hook Best Practices

- **Place hooks at the top level** of function components
- **Use dependency arrays correctly** in useEffect to avoid infinite loops
- **Clean up side effects** in useEffect return functions
- **Create custom hooks** for complex logic that uses multiple built-in hooks
- **Follow hook naming convention** with "use" prefix for custom hooks 
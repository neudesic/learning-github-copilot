# GitHub Copilot Learning Repository - Code Instructions

## 🎯 General Guidelines

- **Always use TypeScript** for new JavaScript/React code with strict mode enabled
- **Prefer functional programming patterns** over imperative approaches
- **Include comprehensive error handling** for all async operations
- **Add JSDoc comments** for all public APIs and complex functions
- **Follow semantic naming conventions** for variables, functions, and files

## 📁 File Organization

- **Use kebab-case** for file and folder names (e.g., `user-profile.tsx`)
- **Group related files** in dedicated folders with index files for exports
- **Separate concerns** with clear boundaries between components, services, and utilities
- **Place tests** adjacent to source files with `.test.ts` or `.spec.ts` extensions

## ⚛️ React Development Standards

- **Use functional components** with React hooks instead of class components
- **Implement proper prop validation** with TypeScript interfaces
- **Extract custom hooks** for reusable stateful logic
- **Use React.memo** for performance optimization when appropriate
- **Implement proper error boundaries** for component error handling

## 🔧 Code Quality

- **Follow SOLID principles** in component and service design
- **Use composition over inheritance** patterns
- **Implement proper separation of concerns** between UI and business logic
- **Add comprehensive unit tests** with minimum 80% coverage
- **Use meaningful variable names** that clearly express intent

## 🚨 Security Practices

- **Validate all user inputs** on both client and server sides
- **Sanitize data** before displaying in UI components
- **Use environment variables** for sensitive configuration
- **Implement proper authentication checks** for protected routes
- **Follow OWASP security guidelines** for web applications

## 📚 Documentation Standards

- **Include README files** for all major features and modules
- **Document API endpoints** with clear request/response examples
- **Add inline comments** for complex business logic
- **Create user guides** for new features and tools
- **Maintain up-to-date setup instructions** for development environment

## 🧪 Testing Requirements

- **Write tests first** for critical business logic (TDD approach)
- **Test user interactions** with React Testing Library
- **Mock external dependencies** in unit tests
- **Include accessibility tests** for UI components
- **Create integration tests** for complete user workflows

## 🎨 Styling Guidelines

- **Use CSS Modules** or styled-components for component styling
- **Follow BEM methodology** for CSS class naming when using regular CSS
- **Implement responsive design** patterns for mobile compatibility
- **Use design tokens** for consistent spacing, colors, and typography
- **Ensure WCAG 2.1 AA compliance** for accessibility standards

## 🔄 State Management

- **Use React Context** for application-wide state management
- **Implement proper state normalization** for complex data structures
- **Use reducers** for complex state update logic
- **Cache API responses** appropriately to reduce network requests
- **Handle loading and error states** consistently across the application

## 📦 Dependencies

- **Prefer established libraries** with active maintenance and community support
- **Keep dependencies updated** and audit for security vulnerabilities
- **Use exact version pinning** for critical dependencies
- **Document reasons** for major dependency choices
- **Minimize bundle size** by avoiding unnecessary dependencies 
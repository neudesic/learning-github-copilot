---
mode: 'agent'
model: Claude Sonnet 4
description: 'Generate comprehensive unit tests for selected code'
tools: ['terminal']
---

# Generate Unit Tests

Generate comprehensive unit tests for the selected code with full coverage and best practices.

## Test Requirements

### 🧪 Testing Framework
- Use **Jest** as the primary testing framework
- Use **React Testing Library** for component testing
- Include **@testing-library/jest-dom** for enhanced matchers
- Add **@testing-library/user-event** for user interaction testing

### 📋 Test Coverage Areas

#### **Functionality Testing**
- Test all **public methods** and **functions**
- Cover all **conditional branches** and **edge cases**
- Test **error handling** and **exception scenarios**
- Validate **return values** and **side effects**

#### **Component Testing** (for React components)
- Test **component rendering** with different props
- Test **user interactions** (clicks, form inputs, etc.)
- Test **state changes** and **effect hooks**
- Test **conditional rendering** scenarios
- Test **accessibility** with jest-axe

#### **Integration Points**
- Mock **external dependencies** appropriately
- Test **API calls** and **async operations**
- Test **event handlers** and **callbacks**
- Test **context providers** and **consumers**

### 🏗️ Test Structure

#### **Organize tests using:**
```javascript
describe('ComponentName/FunctionName', () => {
  describe('when condition', () => {
    it('should do something specific', () => {
      // Test implementation
    });
  });
});
```

#### **Include test categories:**
- **Happy path** scenarios
- **Edge cases** and **boundary conditions**
- **Error scenarios** and **failure cases**
- **Performance considerations** (if applicable)

### 🎯 Test Quality Standards

#### **Best Practices:**
- Use **descriptive test names** that explain the scenario
- Follow **Arrange-Act-Assert** pattern
- Keep tests **independent** and **isolated**
- Use **meaningful assertions** with clear error messages
- Mock **external dependencies** but avoid over-mocking

#### **Accessibility Testing:**
- Test **keyboard navigation**
- Test **screen reader compatibility**
- Test **ARIA labels** and **roles**
- Test **focus management**

#### **Performance Testing:**
- Test **component re-render optimization**
- Test **memory leak prevention**
- Test **async operation cleanup**

## Mock Requirements

### **What to Mock:**
- **External API calls**
- **Browser APIs** (localStorage, fetch, etc.)
- **Third-party libraries**
- **File system operations**
- **Timer functions** (setTimeout, setInterval)

### **What NOT to Mock:**
- **Internal utility functions**
- **React hooks** (unless testing custom hooks)
- **Component props** (test with real data)

## Test File Organization

### **File Naming:**
- Use `.test.tsx` for React component tests
- Use `.test.ts` for utility function tests
- Use `.spec.ts` for integration tests

### **File Location:**
- Place tests **adjacent** to source files
- Use `__tests__` folder for complex test suites
- Create `test-utils.ts` for shared testing utilities

## Additional Requirements

### **Code Coverage:**
- Aim for **minimum 80%** code coverage
- Achieve **100%** coverage for critical business logic
- Include **branch coverage** and **function coverage**

### **Test Documentation:**
- Add **JSDoc comments** for complex test setups
- Include **README** for test suite explanation
- Document **mock strategies** and **test data**

## Context
File/Component being tested: ${selection}
Testing framework preference: ${input:framework:Jest, Vitest, etc.}
Special requirements: ${input:requirements:Any specific testing requirements?}

Reference our testing guidelines in [copilot-instructions.md](../copilot-instructions.md) for additional standards. 
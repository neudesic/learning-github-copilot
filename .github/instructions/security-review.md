---
applyTo:
  - reviewSelection
  - codeGeneration
filePatterns:
  - "**/*.ts"
  - "**/*.js"
  - "**/*.tsx"
  - "**/*.jsx"
  - "**/*.cs"
  - "**/*.py"
keywords:
  - security
  - auth
  - api
  - endpoint
  - validation
  - sanitize
---

# Security Review Instructions

## Authentication & Authorization

- **Verify authentication mechanisms** are properly implemented on all protected endpoints
- **Check authorization levels** ensure users can only access resources they're permitted to
- **Validate JWT tokens** are properly signed, not expired, and contain required claims
- **Review session management** for proper timeout and secure storage
- **Ensure role-based access control** (RBAC) is consistently applied

## Input Validation & Sanitization

- **Validate all user inputs** on both client and server sides
- **Check for SQL injection vulnerabilities** in database queries
- **Review XSS prevention** measures for user-generated content
- **Validate file upload security** including type restrictions and size limits
- **Check parameter tampering protection** for URL and form parameters

## Data Protection

- **Review sensitive data encryption** both at rest and in transit
- **Check password hashing** uses secure algorithms (bcrypt, Argon2)
- **Validate PII handling** complies with privacy regulations
- **Review data retention policies** and secure deletion practices
- **Check for data exposure** in logs, error messages, and debug output

## Network Security

- **Review CORS configuration** for appropriate origin restrictions
- **Check HTTPS enforcement** and secure cookie settings
- **Validate security headers** (CSP, HSTS, X-Frame-Options)
- **Review rate limiting** implementation for API endpoints
- **Check for information disclosure** in HTTP responses

## Error Handling

- **Ensure error messages** don't expose sensitive system information
- **Review exception handling** doesn't leak stack traces in production
- **Check logging practices** don't record sensitive data
- **Validate error codes** provide appropriate information without revealing internals
- **Review debug information** is disabled in production environments 
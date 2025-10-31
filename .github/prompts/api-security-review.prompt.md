---
mode: 'ask'
model: Claude Sonnet 4
description: 'Perform comprehensive REST API security review'
---

# API Security Review

Perform a comprehensive security review of the selected API code and provide actionable recommendations.

## Security Areas to Review

### 🔐 Authentication & Authorization
- Verify **proper authentication** mechanisms are implemented
- Check **authorization controls** for all endpoints
- Review **JWT token validation** and expiration handling
- Ensure **role-based access control** (RBAC) is properly implemented
- Validate **session management** security

### 📝 Input Validation & Sanitization
- Check **input validation** for all user-provided data
- Verify **SQL injection prevention** measures
- Review **XSS protection** for output data
- Validate **file upload security** if applicable
- Check **parameter tampering** protection

### 🚦 Rate Limiting & DoS Protection
- Verify **rate limiting** implementation
- Check **request throttling** mechanisms
- Review **resource exhaustion** protections
- Validate **concurrent request handling**

### 🔍 Error Handling & Information Disclosure
- Review **error message security** (no sensitive data exposure)
- Check **stack trace protection** in production
- Verify **logging practices** don't expose sensitive information
- Validate **HTTP response codes** are appropriate

### 🌐 Network Security
- Review **CORS configuration** and security
- Check **HTTPS enforcement** and TLS settings
- Verify **secure headers** implementation (CSP, HSTS, etc.)
- Validate **API versioning** security considerations

### 🗄️ Data Protection
- Check **sensitive data encryption** at rest and in transit
- Review **password hashing** and storage practices
- Verify **PII handling** compliance
- Validate **data retention** policies

## Output Format

Provide results in the following format:

### 🚨 Critical Issues
List any critical security vulnerabilities that need immediate attention.

### ⚠️ High Priority Issues
List high-priority security concerns that should be addressed soon.

### 📋 Medium Priority Recommendations
List medium-priority improvements and best practices.

### ✅ Security Strengths
Highlight what the API does well from a security perspective.

### 📝 Action Items
Provide a prioritized TODO list with:
- **Issue description**
- **Risk level** (Critical/High/Medium/Low)
- **Remediation steps**
- **Estimated effort**

## Additional Context
API Framework: ${input:framework:What framework/technology is being used?}
Authentication Method: ${input:authMethod:JWT, OAuth, API Keys, etc.}

Reference our security guidelines in [copilot-instructions.md](../copilot-instructions.md) for additional context. 
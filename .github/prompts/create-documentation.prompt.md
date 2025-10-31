---
mode: 'agent'
model: GPT-4.1
description: 'Generate comprehensive documentation for code, APIs, or features'
---

# Create Documentation

Generate comprehensive, user-friendly documentation for the selected code, API, or feature.

## Documentation Type
${input:docType:README, API docs, User guide, Developer guide, etc.}

## Documentation Requirements

### 📝 Content Structure

#### **For README Files:**
- **Project title** and brief description
- **Installation instructions** with prerequisites
- **Quick start guide** with basic examples
- **Usage examples** with code snippets
- **Configuration options** and environment variables
- **Contributing guidelines** and development setup
- **License information** and acknowledgments

#### **For API Documentation:**
- **Endpoint overview** with HTTP methods and URLs
- **Request/response examples** with sample data
- **Authentication requirements** and header format
- **Error codes** and error response formats
- **Rate limiting** and usage guidelines
- **SDK examples** in multiple programming languages

#### **For User Guides:**
- **Step-by-step instructions** with screenshots/diagrams
- **Common use cases** and workflows
- **Troubleshooting section** with FAQs
- **Best practices** and tips
- **Glossary** of terms and concepts

#### **For Developer Guides:**
- **Architecture overview** and design decisions
- **Code organization** and file structure
- **Development workflow** and contribution process
- **Testing strategies** and requirements
- **Deployment procedures** and environments

### 🎨 Formatting Standards

#### **Markdown Best Practices:**
- Use **clear headings** with proper hierarchy (H1, H2, H3)
- Include **table of contents** for longer documents
- Use **code blocks** with syntax highlighting
- Add **badges** for build status, version, license
- Include **screenshots** and **diagrams** where helpful

#### **Code Examples:**
- Provide **working code samples** that can be copy-pasted
- Include **error handling** in examples
- Use **realistic data** in examples, not placeholder text
- Show **before and after** states where relevant
- Include **multiple approaches** for complex scenarios

#### **Visual Elements:**
- Add **emojis** for section headers (📝, 🚀, ⚙️, etc.)
- Use **tables** for structured information
- Include **flowcharts** or **diagrams** for complex processes
- Add **callout boxes** for important notes and warnings

### 🎯 Quality Standards

#### **Clarity and Accessibility:**
- Write in **clear, concise language** avoiding jargon
- Use **active voice** instead of passive voice
- Include **definitions** for technical terms
- Provide **context** for why something is important
- Structure content **logically** from basic to advanced

#### **Completeness:**
- Cover **all major features** and use cases
- Include **edge cases** and limitations
- Provide **troubleshooting** information
- Add **links to related resources**
- Keep information **up-to-date** with current version

#### **User Experience:**
- Start with **quick wins** to get users engaged
- Provide **multiple learning paths** (beginner to advanced)
- Include **search-friendly** headings and keywords
- Add **internal links** for easy navigation
- Test instructions with **fresh eyes**

## Target Audience
${input:audience:Developers, End users, System administrators, etc.}

## Additional Context
- **Technology stack**: ${input:techStack:Languages, frameworks, tools used}
- **Complexity level**: ${input:complexity:Beginner, Intermediate, Advanced}
- **Documentation scope**: ${input:scope:What specific aspects to cover}

## Special Requirements
${input:requirements:Any specific formatting, style, or content requirements}

## References
- Follow our documentation standards in [copilot-instructions.md](../copilot-instructions.md)
- Consider accessibility guidelines for inclusive documentation
- Include links to official documentation for external dependencies

## Output Format
Generate the documentation in **Markdown format** with:
- **Proper heading structure**
- **Working code examples**
- **Clear step-by-step instructions**
- **Helpful visual elements**
- **Professional formatting** 
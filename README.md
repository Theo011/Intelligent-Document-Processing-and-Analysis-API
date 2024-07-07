# Intelligent Document Processing and Analysis API

This API provides advanced document processing and analysis capabilities using locally run Large Language Models (LLMs). It offers a range of features for extracting insights, summarizing content, and comparing documents, demonstrating the integration of local LLMs with a robust .NET backend. The API is designed to work with any LLM server that follows the OpenAI API standard, allowing users to leverage their own hardware and models for document processing tasks.

## Features

- **General LLM Completion**: Utilize the power of Large Language Models for various text generation tasks.
- **Document Summarization**: Generate concise summaries of uploaded documents.
- **Document Analysis**: Perform in-depth analysis of document content.
- **Sentiment Analysis**: Determine the overall sentiment of a document.
- **Dangerous Content Identification**: Identify potentially dangerous or illegal content within documents.
- **Document Comparison**: Compare two documents and highlight similarities and differences.

## Technology Stack

- **Backend Framework**: ASP.NET Core 8.0
  - Leverages the latest features of .NET for high-performance web API development.
  - Uses Entity Framework Core for database operations.
  - Implements dependency injection for loose coupling and better testability.
  - Utilizes the repository pattern for improved data access abstraction.

- **Database**: SQLite
  - Lightweight, serverless database for storing LLM interactions.
  - Easily replaceable with other database systems supported by Entity Framework Core.

- **LLM Integration**: 
  - Compatible with LM Studio and any service following OpenAI's API standard.
  - Implemented using HttpClient for efficient API communication.
  - Flexible design allows easy switching between different LLM providers.

- **Logging**: Serilog
  - Structured logging for better log analysis and management.
  - Configurable to write logs to various sinks (console, file, etc.).

- **API Documentation**: Swagger/OpenAPI
  - Auto-generated API documentation and testing interface.

- **Text Extraction**:
  - iText for PDF processing.
  - DocumentFormat.OpenXml for handling Microsoft Office documents.

- **Configuration Management**:
  - Uses ASP.NET Core's built-in configuration system.
  - Supports environment-specific settings (Development, Production).

- **Dependency Management**: NuGet

## LLM Server and Model

This API is designed to work with any LLM server that follows the OpenAI API standard. During development and testing, the following setup was used:

- **LLM Server**: LM Studio
- **Model**: bartowski/Hermes-2-Theta-Llama-3-8B-GGUF/Hermes-2-Theta-Llama-3-8B-Q6_K.gguf
- **Hardware**: NVIDIA RTX 2080 TI GPU with 11GB VRAM

Users are free to use any compatible LLM server, model, and hardware configuration that suits their needs.

## Usage

The API exposes several endpoints for document processing and analysis:

- `/api/llm/completion`: General LLM completion
- `/api/document/summary`: Document summarization
- `/api/document/analysis`: Document analysis
- `/api/document/sentiment`: Document sentiment analysis
- `/api/document/dangerous`: Identification of dangerous or illegal content
- `/api/document/compare`: Document comparison

Each endpoint accepts document uploads and returns processed results based on the specific analysis requested.

### Supported Document Types

The API currently supports the following document formats:
- PDF (.pdf)
- Microsoft Word (.doc, .docx)
- Plain Text (.txt)

## Configuration

The `appsettings.json` file contains various configuration options, including:

- Database connection string
- LLM API settings (IP, port, endpoint)
- Logging configuration
- Other application-specific settings

Ensure to update these settings according to your environment and LLM server configuration.

## Project Structure

The project is organized into the following key directories:

- `CompletionRequests/`: Contains JSON files defining LLM completion request templates.
- `Controllers/`: Defines API endpoints and request handling.
- `DbContexts/`: Contains the Entity Framework Core database context.
- `Models/`: Defines data models, entities, and DTOs.
- `Profiles/`: Contains AutoMapper profiles for object mapping.
- `Repositories/`: Implements data access logic using the repository pattern.
- `Services/`: Contains business logic and LLM interaction services.
- `Uploads/`: Temporary storage for uploaded documents.
- `Utilities/`: Houses helper classes, constants, and utility functions.

The entry point of the application is the `Program.cs` file in the root directory, which configures and launches the ASP.NET Core web application.

## Database Initialization

This project uses a Code-First approach with Entity Framework Core. To set up the database:

1. Ensure the correct connection string is set in `appsettings.json`.
2. Open the Package Manager Console in Visual Studio.
3. Run the following commands:
   
   ```
   Add-Migration InitialCreate
   Update-Database
   ```

These commands will create the initial database schema based on the entity models.

## Error Handling and Logging

The application uses Serilog for structured logging, capturing detailed information about errors and application events. Logs are configurable to be written to various sinks including console and files. The API returns appropriate HTTP status codes and error messages to clients in case of failures, ensuring robust error handling throughout the application.

## Testing

While the current version focuses on core functionality, the project structure is designed with testability in mind. Future iterations may include unit tests for services and repositories, as well as integration tests for API endpoints to ensure reliability and ease of maintenance.

## Future Improvements

Potential areas for future enhancement include:
- Support for additional document types (e.g., .rtf, .odt)
- Implementation of more advanced NLP tasks
- Optimization of LLM integration for improved performance
- Enhanced caching mechanisms for faster response times
- Expanded API endpoints for more specific document processing tasks
- Implementation of batch processing capabilities for multiple documents
- Advanced error handling and input validation
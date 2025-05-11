# Compulsory 2: Second-Hand E-Commerce Platform Backend

## Design Decisions & Assumptions

### Database Selection

- **MongoDB (NoSQL)** is used for item listings. This choice is justified by the need for a flexible schema to accommodate various item types and attributes, as well as efficient querying and scalability for user-generated content.
- **PostgreSQL (Relational)** is used for structured data such as user profiles, orders, and reviews. This ensures strong consistency, transactional integrity, and support for complex relationships.
- **Redis (NoSQL, In-Memory)** is used for caching frequently accessed data (e.g., listings, search results) to improve performance and reduce database load.

### Data Schema and Storage Strategy

- **Listings (MongoDB):** Each listing is a document with flexible fields for category-specific attributes, images, and metadata. Indexes are created for text search, category, price, and status.
- **Orders (PostgreSQL):** Orders are stored relationally, supporting transactional updates and relationships to users and listings.
- **Reviews (PostgreSQL):** Reviews are linked to orders, users, and targets (seller, buyer, or item), supporting aggregation and analytics.
- **User Profiles (PostgreSQL):** User profiles are implemented in the `users-service` using PostgreSQL. The schema includes fields such as `id`, `username`, `email`, and `avatar_url`. The service provides CRUD operations, CQRS separation, and publishes user deletion events for system-wide consistency.

#### Example Schemas

- Listings: See `listings-service/Models/Listing.cs`
- Orders: See `orders-service/Domain/Models/Order.cs`
- Reviews: See `reviews-service/Domain/Models/Review.cs`

### Integration of Cloud Storage

- **MinIO (S3-compatible):** Used for storing item images and multimedia content.
- **Integration Flow:**
  1. Clients request pre-signed upload URLs from the backend.
  2. Clients upload images directly to MinIO using these URLs.
  3. Listing documents store the URLs of uploaded images.
  4. On listing creation/update, caches are invalidated to ensure consistency.

### Caching Strategy

- **Technology:** Redis
- **Cached Data:** All listings, individual listings, seller listings, and search results.
- **Invalidation:** 
  - Time-based (TTL) for search results.
  - Event-based invalidation on create, update, or delete operations.
  - Write-through and lazy loading patterns are used where appropriate.
- **Implementation:** See `listings-service/Services/ListingService.cs` for cache usage and invalidation logic.

### CQRS Implementation

- **Pattern:** Command Query Responsibility Segregation (CQRS) is implemented across all services.
  - **Commands:** Handle write operations (create, update, delete).
  - **Queries:** Handle read operations (get, search, aggregate).
- **Benefits:** Enables independent scaling of read/write workloads, simplifies business logic, and improves maintainability.
- **Implementation:** Each service (listings, orders, reviews) registers separate command and query handlers.

### Transaction Management

- **PostgreSQL:** Uses native transactions for critical operations (e.g., order placement, review creation).
- **MongoDB:** Uses atomic document updates and, where needed, multi-document transactions.
- **Cross-Service Consistency:** Eventual consistency is maintained between listings (MongoDB) and orders (PostgreSQL). For operations spanning both, a two-phase commit or saga pattern can be implemented (not fully implemented in this codebase, but the architecture supports it).
- **Critical Scenarios:** Order placement, listing status updates, and review creation are handled with transactional integrity.

---

## Implementation Details

- **Microservices:** The backend is split into multiple services (`listings-service`, `orders-service`, `reviews-service`, `users-service`), each with its own database and API.
- **Message Broker:** RabbitMQ is used for inter-service communication and event handling.
- **Cloud Storage:** MinIO is used for storing images, with pre-signed URL support for secure uploads.
- **Caching:** Redis is used for caching listings and search results.
- **CQRS:** All services use CQRS for separation of read/write logic.
- **API Documentation:** OpenAPI is enabled for all services.

## Assumptions

- User authentication and profile management are assumed to be handled by a separate service (not included in this repo).
- Payment processing is out of scope for this implementation.
- The code is structured for extensibility and easy addition of business logic.

# Compulsory 2: Second-Hand E-Commerce Platform Backend

## Project Overview
This project implements the backend architecture for a second-hand item e-commerce platform with the following core functionalities:
- Listing items for sale
- Browsing listings
- Placing orders
- Reviewing sellers

## Architecture Design

### Database Selection

#### MongoDB (NoSQL) - For Listings
We've chosen MongoDB specifically for our item listings for the following reasons:
- **Flexible Schema**: Accommodates varying attributes across different item categories (electronics, clothing, furniture, etc.)
- **Document Structure**: Natural fit for complex item listings with nested attributes and category-specific fields
- **Query Performance**: Strong indexing and search capabilities for browsing and filtering listings
- **Geospatial Support**: Efficient location-based searches for nearby listings

#### PostgreSQL (Relational) - For Core Data
PostgreSQL will serve as our primary database for most data entities:
- **User Profiles**: Account information, preferences, and authentication
- **Orders & Transactions**: Purchase history and payment tracking
- **Reviews & Ratings**: Seller and buyer feedback
- **Inventory Management**: Stock levels and availability tracking
- **Analytics Data**: Structured reporting and business intelligence
- **Financial Records**: Payment processing and accounting

#### Redis (NoSQL) - For Caching and Sessions
Redis serves multiple purposes in our architecture:
- **Caching Layer**: In-memory data store for frequently accessed data
- **Session Management**: Handling user sessions and authentication tokens
- **Rate Limiting**: Protecting APIs from abuse
- **Message Broker**: Supporting event-driven communication between services

### Data Schema and Storage Strategy

#### User Profiles (PostgreSQL)
```json
{
  "id": "string",
  "email": "string",
  "name": "string",
  "avatar_url":"string"
}
```

#### Item Listings (MongoDB)
```json
{
  "_id": "string",
  "sellerId": "string",
  "title": "string",
  "description": "string",
  "category": "string",
  "subcategory": "string",
  "condition": "string",
  "price": "number",
  "currency": "string",
  "location": {
    "city": "string",
    "country": "string",
    "coordinates": [longitude, latitude]
  },
  "images": ["string"], // Cloud storage URLs
  "status": "string", // active, sold, reserved, deleted
  "createdAt": "timestamp",
  "updatedAt": "timestamp",
  "metadata": {
    "views": "number",
    "favorites": "number",
    "custom_attributes": { } // Flexible schema for category-specific attributes
  }
}
```

#### Orders (PostgreSQL)
```json
{
  "id": "string",
  "buyerId": "string",
  "sellerId": "string",
  "listingId": "string",
  "status": "string", // pending, confirmed, shipped, delivered, cancelled, returned
  "amount": "number",
  "currency": "string",
  "paymentId": "string",
  "shipping": {
    "address": { },
    "method": "string",
    "trackingNumber": "string",
    "estimatedDelivery": "date"
  },
  "createdAt": "timestamp",
  "updatedAt": "timestamp"
}
```

#### Reviews (PostgreSQL)
```json
{
  "id": "string",
  "orderId": "string",
  "reviewerId": "string",
  "targetId": "string",
  "targetType": "string", // seller, buyer, item
  "rating": "number",
  "comment": "string",
  "createdAt": "timestamp",
  "updatedAt": "timestamp",
  "helpful": "number" // Number of users who found this review helpful
}
```

### Cloud Storage Integration

We'll use S3-compatible storage with MinIO for image and multimedia content:

- **Storage Organization**:
  - Buckets for different content types (profile-images, listing-images)
  - Object prefixes to organize files hierarchically (user/{userId}/avatars/)
  - Content-addressed storage with hashed object keys to prevent duplicates
  - Versioning enabled for critical content to track changes

- **Integration Flow**:
  1. Client receives pre-signed S3 URLs for direct upload to MinIO
  2. After upload, notification events trigger thumbnail generation and metadata extraction
  3. Database records are updated with S3 object URLs and extracted metadata
  4. CDN layer in front of MinIO distributes content globally for fast access

### Caching Strategy

#### Technology: Redis

#### Cached Data:
- **Hot Listings**: Most viewed and recently added items
- **User Profiles**: Frequently accessed user data (excluding sensitive information)
- **Search Results**: Common search queries and filtered listings
- **Category Navigation**: Category tree and popular filters
- **Static Content**: Platform configuration and static metadata

#### Invalidation Strategy:
- **Time-Based (TTL)**: Default expiration of 1 hour for most cached data
- **Event-Based**: Cache invalidation on relevant data modifications
- **Write-Through**: Critical data updates proactively update the cache
- **Lazy Loading**: Background refresh for soon-to-expire high-value cache items

### CQRS Implementation

Our implementation separates read and write responsibilities:

#### Command Services (Write Operations):
- **Listing Service**: Create, update, delete listings (MongoDB)
- **Order Service**: Place orders, update order status (PostgreSQL)
- **User Service**: Register, update profile, manage preferences (PostgreSQL)
- **Review Service**: Create and manage reviews (PostgreSQL)

#### Query Services (Read Operations):
- **Search Service**: Find items by various criteria
- **Recommendation Service**: Personalized item suggestions
- **Analytics Service**: Viewing patterns and market insights
- **Notification Service**: User alerts and messages

#### Benefits:
- **Independent Scaling**: Scale read and write services based on actual load
- **Optimized Data Models**: Different models for write (normalized) and read (denormalized)
- **Resilience**: Read operations continue working even if write services are down

### Transaction Management

#### Approach:
- **PostgreSQL Transactions**: For critical operations within the relational database
- **Two-Phase Commits**: For operations spanning PostgreSQL and MongoDB
- **Saga Pattern**: For long-running transactions (order processing)
- **Eventual Consistency**: Between MongoDB listings and PostgreSQL order data

#### Critical Transaction Scenarios:
1. **Order Placement**:
   - Verify item availability
   - Reserve inventory
   - Process payment
   - Create order record
   - Notify seller

2. **Item Status Updates**:
   - Validate current status
   - Update listing status
   - Update relevant indexes
   - Notify interested buyers

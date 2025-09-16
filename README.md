# Announcement API

The Announcement API is a RESTful API that enables users to post announcements, add users to their groups, register, and create and manage groups. It was constructed using Clean Architecture principles and ASP.NET Core, EF Core, and JWT authentication.

## Features
- User registration and login system (JWT + Identity)
- Complete CRUD for groups created by users
- Adding or removing people from groups
- Complete CRUD for announcements within one's own group
- Read-only access to other groups' announcements
- Interactive API exploration using Swagger UI

## Technologies
- ASP.NET Core 8
- Entity Framework Core 8 (Code First Approach)
- JWT Authentication & Identity
- Custom Authorization Policies
  
## Database Relationships
- Users and Groups: many-to-many, through GroupUser join table
- Groups and Announcements: one-to-many
- Users and Announcements: one-to-many (user is the creator)

## Policies for Authorization
Custom authorization policies are used by the API to manage resource access:
- AnnouncementReader: Enables users to read announcements to which they have access
- AnnouncementCreator: Enables users to have all CRUD access for their own announcements.
- GroupMember: Provides group members with access to resources tailored to their group.
- GroupAdmin: Provides complete control over group management.

## API Endpoints
### Announcement Controller
- GET /api/announcements/{announcementId} : Get an announcement
- PUT /api/announcements/{announcementId} : Update an announcement
- DELETE /api/announcements/{announcementId} : Delete an announcement

### AppUser Controller
- GET /me/groups : List groups of the current user (with query)
- DELETE /me/groups/{groupId} : Leave a group
- GET /me/announcements : List all announcements of the user (with query)

### Auth Controller
- POST /api/auth/register : Register a new user
- POST /api/auth/login : User login

### Group Controller
- POST /api/groups : Create a new group
- GET /api/groups/{groupId} : Get a group with its announcements and users (with query)
- PUT /api/groups/{groupId} : Update a group
- DELETE /api/groups/{groupId} : Delete a group
- GET /api/groups/{groupId}/users : List users of a group (with query)
- POST /api/groups/{groupId}/users : Add a user to a group
- DELETE /api/groups/{groupId}/users/{userId} : Remove a user from a group
- POST /api/groups/{groupId}/announcements : Add an announcement to a group
- GET /api/groups/{groupId}/announcements : List announcements in a group (with query)

## Sample API Responses
### All API responses follow a custom JSON envelope with the following format:
```json
{
  "success": true | false, // Indicates whether the request was successful
  "data": {...} | [...] | null, // Contains the response data
  "message": "string" | null // Optional message describing the result
}
```
### Example 1: POST /api/login
```json
{
  "success": true,
  "data": {
    "id": "9c3b6a0a-b319-4ba6-be11-e910462b949e",
    "userName": "my user",
    "email": "myuser@mymail.com",
    "token": "mytokenUxMiIsInR5cCI6IkpXVCJ9..."
  },
  "message": "You have logged in successfully."
}
```
### Example 2: POST /api/groups (after logged in)
```json
{
  "success": true,
  "data": { 
    "id": 9,
    "name": "my group"
  }, 
  "message": "The group has been created successfully."
}
```
### Example 3: POST /api/groups/9/announcements (after logged in, if you are the admin of the group)
```json
{
  "success": true,
  "data": {
    "id": 4,
    "title": "my title",
    "content": "my content",
    "lastUpdated": "2025-09-16T10:43:48.88832+03:00",
    "userId": "9c3b6a0a-b319-4ba6-be11-e910462b949e",
    "userName": "my user"
  },
  "message": "The announcement has been added successfully."
}
```
### Example 4: GET /api/groups/9 (after logged in, if you are a member of the group)
```json
{
  "success": true,
  "data": {
    "announcements": [
      {
        "id": 3,
        "title": "my title",
        "content": "my content",
        "lastUpdated": "2025-09-16T09:48:37.7992207",
        "userId": "9c3b6a0a-b319-4ba6-be11-e910462b949e",
        "userName": "maymun"
      }
    ],
    "users": [
      {
        "id": "9a244aed-185c-415b-afb9-32bd50df8a58",
        "userName": "armut"
      },
      {
        "id": "9c3b6a0a-b319-4ba6-be11-e910462b949e",
        "userName": "maymun"
      }
    ],
    "id": 5,
    "name": "my group"
  },
  "message": null
}
```

## Project Structure
The project follows Clean Architecture, divided into four main layers:
### API Layer
- Properties
- Controllers
- Responses
- AnnouncementApi.http
- appsettings.json
- Program.cs
### Application Layer
- Authorization/Requirements
- DTOs
- Interfaces
- Mappers
- Queries
### Domain Layer
- Entities
### Infrastructure Layer
- Authorization/Handlers
- Data
- Migrations
- Repositories
- Services
- DependencyInjection.cs

## Documentation
Swagger UI allows for the exploration and testing of all API endpoints. When the API is running, you can access Swagger at /swagger. 

# Day 05 Caching

I discovered today how to include caching into an ASP.NET Core Web API to enhance performance
by temporarily saving commonly requested data rather than repeatedly requesting the database

## The flow became:

### GET Users

```text
GET Users :
Cache existing?
Cache : Response
Database : Save Cache then Response
```

To activate the in-memory cache, use `AddMemoryCache()`

`IMemoryCache` → Using the Controller's cache

`Get()` retrieves data from the cache

Data is stored in the cache using `Set()`

**Cache Miss** → We go to the database since the data is unavailable

**Cache Hit** → We retrieve the data from the Cache since it is available

**Cache Invalidation** → When data changes, the previous cache is deleted

`Remove("users")` → After adding a user, the previous version is deleted

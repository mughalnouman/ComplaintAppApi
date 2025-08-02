using Microsoft.AspNetCore.Mvc; // For API-related attributes like [ApiController], [Route], etc.
using Microsoft.EntityFrameworkCore; // For async DB operations and LINQ methods
using ComplaintApi.Model;     // Include your userModel class

namespace ComplaintApi.Controllers
{
    // This attribute defines that the controller is an API controller
    [ApiController]

    // Defines the base route of the API: e.g., api/user
    [Route("api/[controller]")]

    public class UserMasterController : ControllerBase // Inherits from ControllerBase (no views, only APIs)
    {
        private readonly ComplaintdbContext _context;

        // Constructor to inject the database context (DbContext)
        public UserMasterController(ComplaintdbContext context)
        {
            _context = context; // Assign context to the private field for later use
        }

        // GET: api/user
        // Fetch all users from the userTable
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<userModel>>> GetAllUsers()
        //{
        //    // Fetch all users asynchronously from database
        //    //userTable is coming from ComplaintdbContext.cs file
        //    return await _context.userTable.ToListAsync();
        //}

        //    [HttpGet]
        //    public async Task<ActionResult<object>> GetAllUsers(
        //string? search = null,
        //int page = 1,
        //int pageSize = 5,
        //string sortBy = "userID",
        //string sortOrder = "asc")
        //    {
        //        if (page <= 0) page = 1;
        //        if (pageSize <= 0) pageSize = 5;

        //        // Start with full queryable dataset
        //        IQueryable<userModel> query = _context.userTable;

        //        // SEARCH
        //        if (!string.IsNullOrEmpty(search))
        //        {
        //            query = query.Where(u =>
        //                u.email.Contains(search) ||
        //                u.mobile.Contains(search) ||
        //                u.city.Contains(search) ||
        //                u.state.Contains(search) ||
        //                u.address.Contains(search));
        //        }

        //        // SORTING
        //        query = sortBy.ToLower() switch
        //        {
        //            "email" => sortOrder == "desc" ? query.OrderByDescending(u => u.email) : query.OrderBy(u => u.email),
        //            "mobile" => sortOrder == "desc" ? query.OrderByDescending(u => u.mobile) : query.OrderBy(u => u.mobile),
        //            "city" => sortOrder == "desc" ? query.OrderByDescending(u => u.city) : query.OrderBy(u => u.city),
        //            "state" => sortOrder == "desc" ? query.OrderByDescending(u => u.state) : query.OrderBy(u => u.state),
        //            "address" => sortOrder == "desc" ? query.OrderByDescending(u => u.address) : query.OrderBy(u => u.address),
        //            _ => sortOrder == "desc" ? query.OrderByDescending(u => u.userID) : query.OrderBy(u => u.userID),
        //        };

        //        // Total count before paging
        //        int totalCount = await query.CountAsync();

        //        // PAGINATION
        //        var users = await query
        //            .Skip((page - 1) * pageSize)
        //            .Take(pageSize)
        //            .ToListAsync();

        //        // Return combined result with data + total count
        //        return Ok(new
        //        {
        //            data = users,
        //            totalCount
        //        });
        //    }
        [HttpGet]
        public async Task<IActionResult> GetUsers(
       string? searchTerm = null,
       string? sortBy = "userID",
       string? sortDirection = "asc",
       int pageNumber = 1,
       int pageSize = 10)
        {
            var query = _context.userTable.AsQueryable();

            // SEARCHING
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u =>
                    u.email.Contains(searchTerm) ||
                    u.mobile.Contains(searchTerm) ||
                    u.city.Contains(searchTerm) ||
                    u.state.Contains(searchTerm) ||
                    u.address.Contains(searchTerm));
            }

            // SORTING
            switch (sortBy.ToLower())
            {
                case "email":
                    query = sortDirection == "desc" ? query.OrderByDescending(u => u.email) : query.OrderBy(u => u.email);
                    break;
                case "mobile":
                    query = sortDirection == "desc" ? query.OrderByDescending(u => u.mobile) : query.OrderBy(u => u.mobile);
                    break;
                case "city":
                    query = sortDirection == "desc" ? query.OrderByDescending(u => u.city) : query.OrderBy(u => u.city);
                    break;
                case "state":
                    query = sortDirection == "desc" ? query.OrderByDescending(u => u.state) : query.OrderBy(u => u.state);
                    break;
                case "address":
                    query = sortDirection == "desc" ? query.OrderByDescending(u => u.address) : query.OrderBy(u => u.address);
                    break;
                default:
                    query = sortDirection == "desc" ? query.OrderByDescending(u => u.userID) : query.OrderBy(u => u.userID);
                    break;
            }

            // PAGINATION
            var totalCount = await query.CountAsync();
            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                users
            });
        }


        // GET: api/user/{id}
        // Get a single user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<userModel>> GetUserById(int id)
        {
            // Find the user in database by primary key
            var user = await _context.userTable.FindAsync(id);

            if (user == null)
            {
                // Return 404 Not Found if user doesn't exist
                return NotFound();
            }

            // Return 200 OK with user data
            return Ok(user);
        }

        // POST: api/user
        // Add a new user (with email and mobile uniqueness check)
        [HttpPost]
        public async Task<ActionResult<userModel>> CreateUser(userModel newUser)
        {
            // Check if email already exists in the database
            bool emailExists = await _context.userTable.AnyAsync(u => u.email == newUser.email);
            // Check if mobile already exists in the database
            bool mobileExists = await _context.userTable.AnyAsync(u => u.mobile == newUser.mobile);
            if (emailExists && mobileExists)
            {
                return BadRequest("Mobile & Email already exists");
            }
            if (mobileExists)
            {
                // Return 400 Bad Request if mobile is duplicate
                return BadRequest("Mobile number already exists.");
            }

            if (emailExists)
            {
                // Return 400 Bad Request if email is duplicate
                return BadRequest("Email already exists.");
            }

            // Add the new user to the DbSet
            _context.userTable.Add(newUser);

            // Save changes asynchronously to the database
            await _context.SaveChangesAsync();

            // Return 201 Created with the new user object
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.userID }, newUser);
        }

        // PUT: api/user/{id}
        // Update an existing user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, userModel updatedUser)
        {
            // Check if the ID in URL matches the model
            if (id != updatedUser.userID)
            {
                return BadRequest("User ID mismatch.");
            }

            // Mark the user as modified
            _context.Entry(updatedUser).State = EntityState.Modified;

            try
            {
                // Save changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If user doesn't exist, return 404
                if (!_context.userTable.Any(e => e.userID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Return 204 No Content on successful update
            return NoContent();
        }

        // DELETE: api/user/{id}
        // Delete a user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Find the user by ID
            var user = await _context.userTable.FindAsync(id);

            if (user == null)
            {
                // Return 404 if not found
                return NotFound();
            }

            // Remove the user from DbSet
            _context.userTable.Remove(user);

            // Save changes to DB
            await _context.SaveChangesAsync();

            // Return 204 No Content
            return NoContent();
        }
    }
}

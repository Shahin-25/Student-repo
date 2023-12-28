
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using user.Data;
using user.Model;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ItemsController(AppDbContext context)
    {
        _context = context;
    }

  
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item1>>> GetItems(bool isActive = true, bool isDeleted = false)
    {
        var items = await _context.Itemslist
            .Where(item => item.IsActive == isActive && item.IsDeleted == isDeleted)
            .ToListAsync();

        return items;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Item1>> GetItem(int id, bool includeDeleted = false)
    {
        var item = includeDeleted
            ? await _context.Itemslist.FindAsync(id)
            : await _context.Itemslist.FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);

        if (item == null)
        {
            return NotFound("Item not found");
        }

        return item;
    }


    [HttpPost]
    public async Task<ActionResult<Item1>> PostItem(Item1 item)
    {
       
        item.IsActive = true;
        item.IsDeleted = false;

        _context.Itemslist.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutItem(int id, Item1 item)
    {
        if (id != item.Id)
        {
            return BadRequest("The provided id does not match the item id.");
        }

       var existingItem = await _context.Itemslist.FindAsync(id);
        if (existingItem == null)
        {
            return NotFound("Item not found");
        }

        item.IsActive = existingItem.IsActive;
        item.IsDeleted = existingItem.IsDeleted;

        _context.Entry(existingItem).State = EntityState.Detached; 

        _context.Entry(item).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
                throw ex;
           
        }

        return Ok("Item Updated Successfully");
    }


       
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await _context.Itemslist.FindAsync(id);
        if (item == null)
        {
            return NotFound("Item not found");
        }

        
        item.IsDeleted = true;
        await _context.SaveChangesAsync();

        return Ok("Item soft deleted Successfully");
    }



    private bool ItemExists(int id)
    {
        return _context.Itemslist.Any(e => e.Id == id);
    }
}


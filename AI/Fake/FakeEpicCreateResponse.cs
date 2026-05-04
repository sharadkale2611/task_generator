namespace task_generator.AI.Fake
{
	public class FakeEpicCreateResponse : IEpicCreateResponse
	{
		public Task<string> Generate(string prompt)
		{
			// TEMP: Hardcoded response (for debugging Epic Builder)
			return Task.FromResult("""
[
  {
    "projectName": "Warehouse Stock Reconciliation System",
    "description": "A web application that helps small startups track physical vs system stock, manage inventory discrepancies, and maintain accurate stock levels with audit logs.",
    "projectCategory": "CRUD",
    "domain": "Inventory",
    "level": "Beginner",
    "estimatedDuration": "2 weeks",
    "techStack": ["React", ".Net Core API", "MSSQL"],
    "status": "Generated",
    "source": "AI"
  },
  {
    "projectName": "Supplier Product Catalog Manager",
    "description": "A system to manage multiple suppliers, their product catalogs, pricing, and availability, allowing startups to maintain an organized inventory sourcing workflow.",
    "projectCategory": "CRUD",
    "domain": "Inventory",
    "level": "Beginner",
    "estimatedDuration": "2 weeks",
    "techStack": ["Angular", ".Net Core MVC", "MSSQL"],
    "status": "Generated",
    "source": "AI"
  },
  {
    "projectName": "Inventory Expiry Tracking Dashboard",
    "description": "An application to track product batches with expiry dates, send alerts for near-expiry items, and manage stock rotation for perishable goods.",
    "projectCategory": "CRUD",
    "domain": "Inventory",
    "level": "Intermediate",
    "estimatedDuration": "3 weeks",
    "techStack": ["React", ".Net Core API", "MySQL"],
    "status": "Generated",
    "source": "AI"
  },
  {
    "projectName": "Multi-Location Inventory Transfer System",
    "description": "A platform that enables businesses to manage stock transfers between multiple warehouses, track transfer history, and ensure accurate inventory synchronization across locations.",
    "projectCategory": "CRUD",
    "domain": "Inventory",
    "level": "Intermediate",
    "estimatedDuration": "3-4 weeks",
    "techStack": ["Angular", ".Net Core API", "MSSQL"],
    "status": "Generated",
    "source": "AI"
  }
]
""");
		}
	}
}
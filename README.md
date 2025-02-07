<h2>📌 Technical Test Description</h2>

<p>This project is a <strong>technical test</strong> focused on managing Overmontor (supervisors) and Montorer (technicians) using <strong>raw SQL queries</strong> for database operations instead of Entity Framework. The application includes:</p>

<ul>
    <li><strong>CRUD operations</strong> for Overmontør (supervisors) and Montorer (technicians).</li>
    <li><strong>Assignment and removal of Montorer (technicians)</strong> while maintaining a many-to-many relationship.</li>
    <li><strong>Validation to prevent data inconsistencies</strong>, ensuring a Montor cannot be removed if they are only assigned to one Overmontor.</li>
    <li><strong>Direct SQL execution</strong> via a data access layer to handle database interactions efficiently.</li>
</ul>

<p>The project is built using <strong>ASP.NET Core MVC</strong> with a <strong>custom SQL-based data access layer</strong> for database management.</p>

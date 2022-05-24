using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;
using Microsoft.Data.SqlClient;

namespace ProjectManager.Controllers
{
    [Route("api")]
    [ApiController]
    public class ProjectManagerController : ControllerBase
    {
        private SqlConnection db;

        public ProjectManagerController()
        {
            string Connect = "Data Source=DESKTOP-BUIS7DE;Database=projectmanager;Integrated Security = True";
            db = new SqlConnection(Connect);
            db.Open();
        }

        [HttpPost("projectAdd")]
        public ActionResult AddProject(ProjectAddResponse par) {
            string sql;
            SqlDataReader reader;
            SqlCommand command;

            int id = GetMaxIdFromTable("projects");
            if (id == -1) return BadRequest();

            sql = $"INSERT INTO projects (id, text, active) VALUES ({id}, '{par.text}', '1')";
            command = new SqlCommand(sql, db);
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("projectEditText")]
        public ActionResult EditProjectText(textEditResponse ter) {
            string sql = "UPDATE projects SET text='" + ter.text + "' WHERE id=" + ter.id;
            SqlCommand command = new SqlCommand(sql, db);
            SqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("projectDelete/{id}")]
        public ActionResult DeleteProject(int id)
        {
            string sql = "DELETE from projects WHERE id=" + id;
            SqlCommand command = new SqlCommand(sql, db);
            SqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("projectActive/{id}/{active}")]
        public ActionResult ProjectActive(int id, bool active)
        {
            string sql;
            if (active) sql = "UPDATE projects SET active='" + 1.ToString() + "' WHERE id=" + id;
            else sql = "UPDATE projects SET active='" + 0 + "' WHERE id=" + id;
            SqlCommand command = new SqlCommand(sql, db);
            SqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("taskAdd")]
        public ActionResult AddTask(TaskAddResponse task)
        {
            string sql;
            SqlDataReader reader;
            SqlCommand command;

            int id = GetMaxIdFromTable("tasks");
            if (id == -1) return BadRequest();

            sql = $"INSERT INTO tasks (id, text, active, projectId) VALUES ({id}, '{task.text}', '1', {task.projectId})";
            command = new SqlCommand(sql, db);
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("taskDelete/{id}")]
        public ActionResult DeleteTask(int id)
        {

            string sql = "DELETE from tasks WHERE id=" + id;
            SqlCommand command = new SqlCommand(sql, db);
            SqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("taskActive/{id}/{active}")]
        public ActionResult TaskActive(int id, bool active)
        {
            string sql;
            if (active) sql = "UPDATE tasks SET active='" + 1.ToString() + "' WHERE id=" + id;
            else sql = "UPDATE tasks SET active='" + 0 + "' WHERE id=" + id;
            SqlCommand command = new SqlCommand(sql, db);
            SqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("taskEditText")]
        public ActionResult EditTaskText(textEditResponse ter)
        {
            string sql = "UPDATE tasks SET text='" + ter.text + "' WHERE id=" + ter.id;
            SqlCommand command = new SqlCommand(sql, db);
            SqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        public int GetMaxIdFromTable(string tableName)
        {
            string sql;
            SqlDataReader reader;
            SqlCommand command;

            sql = "SELECT MAX(id) FROM " + tableName;
            command = new SqlCommand(sql, db);
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return -1;
            }
            reader.Read();
            int id;
            try
            {
                if (reader[0].GetType().ToString() == "System.DBNull") id = 0;
                else id = (int)reader[0];
            }
            catch
            {
                return -1;
            }
            reader.Close();
            return id + 1;
        }

        [HttpPost("timeAdd")]
        public ActionResult AddTime(Time time)
        {
            string sql;
            SqlDataReader reader;
            SqlCommand command;

            int id = GetMaxIdFromTable("times");
            if (id == -1) return BadRequest();

            sql = $"INSERT INTO times (id, text, taskId, date, numberOfHours) VALUES ({id}, '{time.text}', {time.taskId}, '{time.date}', {time.numberOfHours})";
            command = new SqlCommand(sql, db);
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("GetProjects")]
        public async Task<ActionResult<List<Project>>> GetProjects() {
            List<Project> projects = new List<Project>();
            string sql = "SELECT * FROM projects";
            SqlCommand command = new SqlCommand(sql, db);
            SqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return projects;
            }
            while (reader.Read())
            {
                Project s = new Project();
                s.id = (int)reader["id"];
                s.text = (string)reader["text"];
                s.active = (string)reader["active"] == "1" ? true : false;
                projects.Add(s);
            }
            reader.Close();
            return projects;
        }

        [HttpGet("GetTasks/{id}")]
        public async Task<ActionResult<List<PTask>>> GetTasks(int id)
        {
            List<PTask> tasks = new List<PTask>();
            string sql = "SELECT * FROM tasks WHERE projectId=" + id.ToString();
            SqlCommand command = new SqlCommand(sql, db);
            SqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return tasks;

            }
            while (reader.Read()) {
                PTask task = new PTask();
                task.id = (int)reader["id"];
                task.text = (string)reader["text"];
                task.active = (string)reader["active"] == "1" ? true : false;
                task.projectId = (int)reader["projectId"];
                tasks.Add(task);
            }

            reader.Close();
            return tasks;
        }

        [HttpGet("GetTimes/{id}")]
        public async Task<ActionResult<List<Time>>> GetTimes(int id)
        {
            List<Time> times = new List<Time>();
            string sql = "SELECT * FROM times WHERE taskId=" + id.ToString();
            SqlCommand command = new SqlCommand(sql, db);
            SqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                return times;
            }
            while (reader.Read())
            {
                Time time = new Time();
                time.id = (int)reader["id"];
                time.text = (string)reader["text"];
                time.taskId = (int)reader["taskId"];
                time.date = (string)reader["date"];
                time.numberOfHours = (int)reader["numberOfHours"];
                times.Add(time);
            }

            reader.Close();
            return times;
        }
    }
}

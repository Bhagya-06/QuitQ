import { useEffect, useState } from "react";
import api from "../../api/axios";
import { showSuccess, showError } from "../../utils/toast";

function Users() {

  const [users, setUsers] = useState([]);

  const fetchUsers = async () => {
    try {

      const res = await api.get("/admin/users");

      setUsers(res.data.data);

    } catch (err) {
      showError(err.response?.data?.message);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const deleteUser = async (id) => {

    if (!window.confirm("Delete user?")) return;

    try {

      await api.delete(`/admin/user/${id}`);

      showSuccess("User deleted");

      fetchUsers();

    } catch (err) {
      showError(err.response?.data?.message);
    }
  };

  return (
    <div className="container mt-5">

      <h2 className="mb-4">Users</h2>

      <table className="table table-bordered">

        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Email</th>
            <th>Username</th>
            <th>Action</th>
          </tr>
        </thead>

        <tbody>

          {users.map(user => (

            <tr key={user.id}>

              <td>{user.id}</td>
              <td>{user.name}</td>
              <td>{user.email}</td>
              <td>{user.username}</td>

              <td>
                <button
                  className="btn btn-danger btn-sm"
                  onClick={() => deleteUser(user.id)}
                >
                  Delete
                </button>
              </td>

            </tr>

          ))}

        </tbody>

      </table>

    </div>
  );
}

export default Users;
import pyodbc

class Database:
    @staticmethod
    def get_connection():
        return pyodbc.connect(
            "DRIVER={ODBC Driver 17 for SQL Server};"
            "SERVER=(localdb)\\MSSQLLocalDB;"
            "DATABASE=QuitQ;"
            "Trusted_Connection=yes;"
        )
    
class DatabaseConnection:
    def __enter__(self):
        self.conn = Database.get_connection()
        return self.conn

    def __exit__(self, exc_type,exc_value,traceback):
        self.conn.close()
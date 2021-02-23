import pyodbc


class DatabaseConfig:
    serverName = ""
    database = ""
    userId = ""
    userPwd = ""

    def __init__(self):
        self.serverName = "172.17.0.62\\SKPMSSQLSERVER,1433"
        self.database = "JobAgentDB"
        self.userId = "job_agent"
        self.userPwd = "4Ndet0wn"

    @property
    def connection(self):
        """
        Attempts to connect to the database.
        @return: A new object of the connection.
        """
        conn = pyodbc.connect('Driver={SQL Server Native Client 11.0};'
                              f'Server={self.serverName};'
                              f'Database={self.database};'
                              f'UID={self.userId};'
                              f'PWD={self.userPwd};')
        return conn

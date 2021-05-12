from pyodbc import Cursor, Connection, connect, DatabaseError, Error
import logging as log


class DatabaseMSSQL(object):
    """
    Represents a class handling the connection to an MSSQL Database.
    """
    __app_config: object
    __connection_string: str

    def __init__(self, app_config: object):
        self.__app_config = app_config
        self.__build_connection_string()

    def connect(self,
                auto_commit: bool = False,
                database_scheme: str = 'JobAgentDB_v2') -> Cursor:
        """
        Instantiates a new connection to the database.
        Returns:
            object: Returns a pyodbc cursor with the created connection.
        """
        cursor: Cursor
        conn: Connection

        try:
            if self.__connection_string == '':
                raise ValueError('Connection string was not fulfilled.')
            else:
                conn = connect(
                    self.__connection_string,
                    autocommit=auto_commit,
                    database=database_scheme)
                with conn:
                    cursor = conn.cursor()
                    return cursor
        except ValueError as er:
            log.info(er)
        except DatabaseError:
            log.warning(f'Error while attempting to connect to <{database_scheme}>')
        except Error as err:
            sqlstate = err.args[0]
            if sqlstate == '08004':
                log.critical(f'Server rejected connection while using Database <{database_scheme}>.')
            if sqlstate == '28000':
                log.critical(f'Invalid authorization specification for Database <{database_scheme}>')
                # log.critical(f'Cannot open <{database_scheme}> by the requested login.')

    def __build_connection_string(self):
        """
        Builds the configuration for the database connection.

        Returns: None.

        """
        conn_key = 'ConnectionString'

        if conn_key in self.__app_config:
            database_config = self.__app_config[conn_key]

            if 'UID' in database_config and 'PWD' in database_config:
                self.__connection_string = f'DRIVER={database_config["Driver"]};' \
                                           f'SERVER={database_config["Server"]};' \
                                           f'UID={database_config["UID"]};' \
                                           f'PWD={database_config["PWD"]};' \
                                           f'TRUSTED_CONNECTION={database_config["Trusted_Connection"]};'
                return

            self.__connection_string = f'DRIVER={database_config["Driver"]};' \
                                       f'SERVER={database_config["Server"]};' \
                                       f'TRUSTED_CONNECTION={database_config["Trusted_Connection"]};'
            return

        raise ValueError(f'Could not build connection, no such key <{conn_key}>.')

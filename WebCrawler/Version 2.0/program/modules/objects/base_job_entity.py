class BaseJobEntity(object):
    """
    An object containing the base of a job object entity.
    """

    def __init__(self, entity_id: int):
        """
        Instantiates a base entity.
        Args:
            entity_id:
        """
        self._entity_id = entity_id

    @property
    def get_entity_id(self):
        """
        Get the unique identifier of this object.
        Returns:
            int: An ID identifying the object.

        Raises:
            ValueError: If the ID is not type of int.

        """
        return self._entity_id

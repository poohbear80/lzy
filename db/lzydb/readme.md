#LzyDb

This document is ment for brainstorming around the project.

We must identify the main features of the projects. 
The main constraints and clearifications around the ideas. 

- MVCC
    - Data will never be updated write a new record for every update. 
- Multithreaded read
- Singlethreaded write
- Data is stored as JSON/Text
- Datarecord is defined as
    - {"Id":"someId","version":1,"previouseLocation":0 data:"some json serialized string"}
    - 1 file for each type
    - Could support in memory db
- Index is kept in seperate file pr type
    - IndexRecord:
        - {key:"someKey", "location":12348 }
    - User should be able to generate custome indices
    - Index key could be provided as function. 
- Map/Reduce functionality
- Cluster?
    - Probably not


User queue for writeing. 

Thread signaling.


 

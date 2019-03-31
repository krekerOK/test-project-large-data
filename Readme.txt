This is simplified version of project, so there is no any safety checkers against user's actions.
E. g.: 
- If user inputs a string when number is expected then program will be crashed.
- If user tries to generate transactional data without having reference data then program will be crashed as well.
- Before generation of transactional data table will be truncated.
- ...

Setup:
1) Setup database. In SQLQueries folder there is InstantiateDB.sql script
2) Connection string to database is hard coded. Definition can be found in TestAssembly.Utils.DataBaseHelper ConnectionString property.
3) Setup file system. All paths are hard coded, program expects that user have 2 folders: 'C:\Data' and 'C:\Data\TransactionData'
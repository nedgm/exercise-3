Being faced with GitHub limitations on the size of a source file, which is <= 100 MB,
it was decided to reduce the size of the FIX message source file to just one record rather than trying to make use of tools like Git LFS.
The reason for that decision is that the limitations set in the parent repository are unknown.
However, if it is critical to have a full-sized message source in the repo, do not hesitate to contact me.

In order for the application to work with the full data set, you will only need to replace the file /FixMessagesApi/data/messages with a suitable one.

Once the database is in place, it will be used by the application further on, migrations are not supported at this stage.
If you want to initialize the database using a different dataset, simply drop fixMessages.db file and start the application.
using System;

namespace Palavyr.BackupAndRestore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // https://severalnines.com/database-blog/running-multiple-postgresql-instances-single-host
                
            // 1. create a temp directory
            // 2. download the latest db archive from S3
            // 3. unzip the archive to create 3 .sql files
            // 4. restore databases (using the alternate name)
            // 5. If any errors are encounters, email paul.e.gradie
            // 6. Delete all alternate databases just created
            // 7. Delete temp directory
        }
    }
}
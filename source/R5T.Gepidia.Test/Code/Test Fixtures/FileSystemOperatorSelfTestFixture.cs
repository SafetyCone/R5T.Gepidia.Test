using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using R5T.Lombardy;
using R5T.Magyar;


namespace R5T.Gepidia.Test
{
    /// <summary>
    /// Uses the <see cref="IFileSystemOperator"/> instance to test itself. I.e. uses <see cref="IFileSystemOperator"/> functionality to test other <see cref="IFileSystemOperator"/> functionality.
    /// Ensures that the root directory exists, but the derived test class is reponsible for root directory cleanup.
    /// </summary>
    /// <remarks>
    /// Because the file-system is so fundamental, it is hard to independently verify operations performed on it.
    /// For example, using <see cref="System.IO"/>-based functionality will work to test operations performed by a local file-system operator, but what about a remote SFTP-based file-system operator?
    /// This test-fixture is meant to test any <see cref="IFileSystemOperator"/> implementation, regardless of file-system location, operating system, etc., just as if the file-system was any other in-memory type.
    /// One way that suggests itself to test file-system operator operation implementations is to use other operation implementations provided by the *same* file system operator!
    /// While, yes, an adversarial test-instance implementation could walk, talk, and squak like a file-system without *actually* interfacing with a file-system, to pass all tests it would have to basically *be* a file system!
    /// </remarks>
    public abstract class FileSystemOperatorSelfTestFixture
    {
        #region Test-Fixture

        public IFileSystemOperator FileSystemOperator { get; }
        public string RootDirectoryPath { get; }
        public IStringlyTypedPathOperator StringlyTypedPathOperator { get; }


        public FileSystemOperatorSelfTestFixture(IFileSystemOperator fileSystemOperator, string rootDirectoryPath, IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            this.FileSystemOperator = fileSystemOperator;
            this.RootDirectoryPath = rootDirectoryPath;
            this.StringlyTypedPathOperator = stringlyTypedPathOperator;

            // Ensure the root directory is created.
            this.FileSystemOperator.CreateDirectoryOnlyIfNotExists(this.RootDirectoryPath);
        }

        #endregion


        /// <summary>
        /// Tests that a file can be created.
        /// </summary>
        [TestMethod]
        public void CreateFileBasic()
        {
            var fileName = PathHelper.GetRandomFileName();
            var filePath = this.StringlyTypedPathOperator.GetFilePath(this.RootDirectoryPath, fileName);

            // Ensure deleted.
            this.FileSystemOperator.DeleteFileOnlyIfExists(filePath);

            // Create the file.
            using (var file = this.FileSystemOperator.CreateFile(filePath))
            {
                // Do nothing.
            }

            // Ensure created.
            var fileExists = this.FileSystemOperator.ExistsFile(filePath);

            Assert.AreEqual(true, fileExists);
        }
    }
}

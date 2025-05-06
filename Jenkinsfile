pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --no-restore'
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test --no-build --verbosity normal'
            }
        }

        // Optional: Add Publish step if needed
        // stage('Publish') {
        //     steps {
        //         bat 'dotnet publish -c Release -o ./publish'
        //     }
        // }
    }
}

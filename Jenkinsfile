pipeline {
    agent any
    stages {
        stage('Install Dependencies') {
            steps {
                bat 'npm install'
            }
        }
        stage('Lint & Test') {
            steps {
                bat 'npm run lint'
                bat 'npm test'
            }
        }
        stage('Build') {
            steps {
                bat 'npm run build'
            }
        }
    }
}

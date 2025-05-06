pipeline {
    agent any
    stages {
        stage('Checkout') {
            steps {
                git 'https://github.com/AimanHaikall/BlogPostCategory.API.git'
            }
        }
        stage('Install Dependencies') {
            steps {
                sh 'npm install'
            }
        }
        stage('Lint & Test') {
            steps {
                sh 'npm run lint'
                sh 'npm test'
            }
        }
        stage('Build') {
            steps {
                sh 'npm run build'
            }
        }
    }
}


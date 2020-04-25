module.exports = {
    plugins: [
        '@semantic-release/commit-analyzer',
        '@semantic-release/release-notes-generator',
        '@semantic-release/github',
        '@semantic-release/exec',
        '@semantic-release/git'
    ],
    verifyConditions: [
        () => {
            if (process.env.NUGET_TOKEN === null || process.env.NUGET_TOKEN === '$(NUGET_TOKEN)')
                throw new Error('The environment variable NUGET_TOKEN is required.')
        },
        '@semantic-release/changelog',
        '@semantic-release/git',
        '@semantic-release/github',
        { 
            path: '@semantic-release/exec',
            cmd: 'chmod +x build.sh && chmod +x publish.sh && ls -l'
        }
    ],
    prepare: [
        '@semantic-release/changelog',
        { 
            path: '@semantic-release/exec',
            cmd: './build.sh ${nextRelease.version}'
        },
        {
            path: '@semantic-release/git',
            assets: [ '**/*.csproj', '!**/*.Tests.csproj', 'CHANGELOG.md' ],
            message: 'chore(release): ${nextRelease.version} [skip ci]\n\n${nextRelease.notes}'
        }
    ],
    publish: [
        {
            path: '@semantic-release/github',
            assets: '**/*.nupkg'
        },
        { 
            path: '@semantic-release/exec',
            cmd: './publish.sh ${process.env.NUGET_TOKEN}'
        }
    ]
}

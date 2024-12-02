Param (
    [switch]$all,
    [string]$filter,
    [switch]$help
)

# Define the base branch (e.g., main or master)
$BASE_BRANCH = "main"

# Function to display help message
function Show-Help {
    Write-Output "Usage: .\New-ChangeSet.ps1 [--all] [--filter <regex>] [--help]"
    Write-Output ""
    Write-Output "Options:"
    Write-Output "  --all             Include 'Chores' and 'Other' sections in the output."
    Write-Output "  --filter <regex>  Filter out commits matching the given regular expression."
    Write-Output "  --help            Show this help message and exit."
}

# Parse command-line arguments
$show_all = $false
$filter_regex = ""

if ($help) {
    Show-Help
    exit 0
}

if ($all) {
    $show_all = $true
}

if ($filter) {
    $filter_regex = $filter
}

# Get the current branch name
$current_branch = git rev-parse --abbrev-ref HEAD

# Extract the issue identifier from the branch name
$issue_identifier = if ($current_branch -match '(patch|feature)/[a-z0-9]+-[0-9]+') { $matches[0] -match '[A-Z]+-[0-9]+'; $matches[0] }

# Fetch the commit messages from the current branch that are not in the base branch and reverse the order
$commits = git log "$BASE_BRANCH..HEAD" --pretty=format:"%s" | ForEach-Object { $_ } | Sort-Object { $_ } -Descending

# Initialize the changeset
$changeset = ""

# Initialize variables for each conventional commit type
$docs_commits = ""
$feat_commits = ""
$fix_commits = ""
$test_commits = ""
$chore_commits = ""
$ci_commits = ""
$other_commits = ""

# Filter and format the commits
foreach ($commit in $commits) {
    # Apply filter if specified
    if ($filter_regex -ne "" -and $commit -match $filter_regex) {
        continue
    }

    # Extract the conventional commit type and the message
    if ($commit -match '^(Merge.*|.*\(#\d+\))$') {
        continue
    }

    if ($commit -match '^(feat|fix|docs|test|chore|ci)(\([^\)]+\))?:\s*(.*)$') {
        $commit_type = $matches[1]
        $commit_message = $matches[3]
        switch ($commit_type) {
            "docs" { $docs_commits += "- $commit_message`n" }
            "feat" { $feat_commits += "- $commit_message`n" }
            "test" { $test_commits += "- $commit_message`n" }
            "fix" { $fix_commits += "- $commit_message`n" }
            "chore" { $chore_commits += "- $commit_message`n" }
            default { $other_commits += "- $commit_message`n" }
        }
    }
    else {
        if ($show_all) {
            $other_commits += "- $commit`n"
        }
    }
}

# Format the changeset
if ($feat_commits -ne "") {
    $changeset += "### Features:`n`n$feat_commits`n"
}
if ($fix_commits -ne "") {
    $changeset += "### Fixes:`n`n$fix_commits`n"
}
if ($test_commits -ne "") {
    $changeset += "### Tests:`n`n$test_commits`n"
}
if ($docs_commits -ne "") {
    $changeset += "### Documentation:`n`n$docs_commits`n"
}
if ($ci_commits -ne "") {
    $changeset += "### Continuous Integration:`n`n$ci_commits`n"
}
if ($show_all) {
    if ($chore_commits -ne "") {
        $changeset += "### Chores:`n`n$chore_commits`n"
    }
    if ($other_commits -ne "") {
        $changeset += "### Other:`n`n$other_commits`n"
    }
}

# Find the first pull_request_template.md file in the repository
$template_file = Get-ChildItem -Recurse -Filter "pull_request_template.md" -Force | Select-Object -First 1

# Check if the template file was found
if (-not $template_file) {
    Write-Warning "pull_request_template.md file not found in the repository."
}
else {
    # Read the pull request template
    $template_content = Get-Content -Path $template_file.FullName -Raw
}

# Append updated content to end of the template
$updated_content = $template_content + $changeset

# Output the updated content
Write-Output $updated_content
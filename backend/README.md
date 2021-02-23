# Bastion of Shadows Server-Side

## Uncovered Code

I strive for 100% coverage of Bastion of Shadows' backend. However, even the best codebase has uncovered lines. Here, you can see exactly why each uncovered line is uncovered.

### Uncovered lines
#### Backend

##### Controllers.UsersController.AddUserToRole

- `var userRoles = user.Roles.Select(r => r.Name);` I'm not sure exactly why this line isn't covered. Typically, if an uncoverable line is implicitly covered by a subsequent line, it's considered covered. However, while this line is covered by the next line implicitly, it isn't considered covered here. However, this line doesn't represent an untested portion of the code, so it doesn't contribute to potential bugs.
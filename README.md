# Calzadonia
Employee time control, display of worked hours of each employee. The ability to create an account for each individual employee. Login and password for admin access : admin password 123

In the Table folder, the Admin file, change the link to your Firebase

Don't foget add rules into your Real Database(Firebase) 
```
{
  /* Visit https://firebase.google.com/docs/database/security to learn more about security rules. */
  "rules": {
    ".read": "auth==null",
    ".write": "auth==null"
  }
}
```

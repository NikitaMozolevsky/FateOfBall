

public class SphereDraft
{
    /*public void MoveSphereWithVelocity(Rigidbody sphereRigitbody, GameObject platform)
    {
        if (SphereController._instance.IsLeft)
        { // Движение в лево
            Vector3 targetVelocity = platform.transform.forward
                                     * SphereController._instance.SphereSpeed;
            sphereRigitbody.velocity = targetVelocity;
        }
        else
        { // Движение в право
            Vector3 targetVelocity = platform.transform.right
                                     * SphereController._instance.SphereSpeed;
            sphereRigitbody.velocity = targetVelocity;
        }
    }*/ 

    /*// Movement with force
    public void MoveSphereWithForce(Rigidbody sphereRigitbody, GameObject platform)
    {
        float speed = SphereController._instance.SphereSpeed;

        // Определяем направление движения в зависимости от выбранной стороны
        Vector3 targetDirection;
    
        if (SphereController._instance.IsLeft)
        {
            targetDirection = platform.transform.forward;
            sphereRigitbody.constraints = RigidbodyConstraints.FreezePositionX;
        }
        else
        {
            targetDirection = platform.transform.right;
            sphereRigitbody.constraints = RigidbodyConstraints.FreezePositionZ;
        }

        // Определяем целевую силу
        Vector3 targetForce = targetDirection * speed;

        // Используем AddForce для применения моментального ускорения
        sphereRigitbody.AddForce(targetForce, ForceMode.Acceleration);
    }*/
}
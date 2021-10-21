Shader "Custom/MaskedObjectShader"
{
    
    SubShader
    {
        

        Pass
        {
            
            Stencil
            {
                
                Ref 1
                Comp Equal
            }
        
        }

        
    }
}

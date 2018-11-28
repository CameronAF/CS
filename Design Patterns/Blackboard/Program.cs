using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// Artificial intelligence pattern for combining disparate sources of data
/// provides a computational framework for the design and implementation of systems that integrate large and diverse specialized modules, 
/// and implement complex, non-deterministic control strategies
/// 
/// The blackboard model defines three main components:
///  - blackboard - a structured global memory containing objects from the solution space
///  - knowledge sources - specialized modules with their own representation
///  - control component - selects, configures and executes modules
///  
/// Step 1) design the solution space(i.e.potential solutions) that leads to the blackboard structure.
///      Then, knowledge sources are identified. These two activities are closely related.[2]
/// Setp 2) The next step is to specify the control component; 
///      it generally takes the form of a complex scheduler that makes use of a set of domain-specific heuristics to rate the relevance of executable knowledge sources
///      
/// The blackboard pattern provides effective solutions for designing and implementing complex systems where heterogeneous modules have to be dynamically combined to solve a problem. This provides non-functional properties such as:
///     reusability
///     changeability
///     robustness.[2]
/// The blackboard pattern allows multiple processes to work closer together on separate threads, polling and reacting when necessary
namespace Blackboard
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
